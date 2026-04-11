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


builder.Logging.ClearProviders();


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
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource=> resource.AddService(DiagnosticsConfig.ServiceName))
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation();
        metrics.AddMeter(DiagnosticsConfig.Meter.Name);
        metrics.AddOtlpExporter(options => options.Endpoint = new Uri("http://localhost:4317"));
    })
    .WithTracing( tracing =>
    {   tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddEntityFrameworkCoreInstrumentation();

        tracing.AddOtlpExporter(options => options.Endpoint = new Uri("http://localhost:4317"));
    });




                  
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