using Microsoft.EntityFrameworkCore;
using Serilog;
using UberMonolith.API.Infrastructure.Data;
using UberMonolith.API.Mappings;
using UberMonolith.API.Repositories;
using StackExchange.Redis;
using UberMonolith.API;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using Serilog.Sinks.Grafana.Loki;
using Confluent.Kafka;

var builder = WebApplication.CreateBuilder(args);
var connectionString =
    builder.Configuration.GetConnectionString("PostgreSqlConnection")
        ?? throw new InvalidOperationException("Connection string"
        + "'PostgreSqlConnection' not found.");

var logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.GrafanaLoki(
        "http://localhost:3100",
        labels: new List<LokiLabel>
        {
            new() { Key = "app",     Value = "uber-backend" },
            new() { Key = "env",     Value = builder.Environment.EnvironmentName }
        })
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(System.Net.IPAddress.Any, 5000);
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("Dev", policy =>
    {
        policy
            .WithOrigins("http://0.0.0.0:5500", "http://localhost:5500")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IRideRepository, RideRepository>();
builder.Services.AddScoped<IDriverRepository,DriverRepository>();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")));
builder.Services.AddScoped(c => c.GetRequiredService<IConnectionMultiplexer>().GetDatabase());
builder.Services.AddScoped<IRideService, RideService>();
builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddKafkaProducer(builder.Configuration);
builder.Services.AddSignalR();

builder.Services.AddSingleton<IProducer<string, string>>(sp =>
{
    var config = sp.GetRequiredService<ProducerConfig>();
    return new ProducerBuilder<string, string>(config).Build();
});
builder.Services.AddKafkaConsumer(builder.Configuration);
builder.Services.AddSingleton<IConsumer<string, string>>(sp =>
{
    var config = sp.GetRequiredService<ConsumerConfig>();
    return new ConsumerBuilder<string, string>(config).Build();
});
 builder.Services.AddHostedService<TripRequestedConsumer>();



var otel = builder.Services.AddOpenTelemetry();

// Configure OpenTelemetry Resources with the application name
otel.ConfigureResource(resource => resource
    .AddService(serviceName: builder.Environment.ApplicationName));

// Add Metrics for ASP.NET Core and our custom metrics and export to Prometheus
otel.WithMetrics(metrics => metrics
    // Metrics provider from OpenTelemetry
    .AddAspNetCoreInstrumentation()
    // Metrics provides by ASP.NET Core in .NET 8
    .AddMeter("Microsoft.AspNetCore.Hosting")
    .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
    // Metrics provided by System.Net libraries
    .AddMeter("System.Net.Http")
    .AddMeter("System.Net.NameResolution")
    .AddPrometheusExporter());
otel.WithTracing(tracing => tracing
    .AddAspNetCoreInstrumentation()
    .AddEntityFrameworkCoreInstrumentation()  
    .AddRedisInstrumentation()                
    .AddOtlpExporter(opts =>
    {
        opts.Endpoint = new Uri("http://localhost:4317"); 
    }));   

                  
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<UberMonolith.API.ExceptionHandlerMiddleware>();
app.UseCors("Dev");
app.MapHub<TripHub>("/tripHub");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
// Configure the Prometheus scraping endpoint
app.MapPrometheusScrapingEndpoint();

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStopping.Register(() =>
{
    var producer = app.Services.GetRequiredService<IProducer<string, string>>();
    var consumer = app.Services.GetRequiredService<IConsumer<string, string>>();
    producer.Flush(TimeSpan.FromSeconds(10)); 
    producer.Dispose();
    consumer.Close();
});
app.Run();