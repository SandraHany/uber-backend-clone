using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using Uber.Trip.Infrastructure.Persistence;

namespace Uber.Trip.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PostgresConnection");
            services.AddDbContext<TripDbContext>(options =>
                options.UseNpgsql(connectionString));

            return services;

        }

        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using TripDbContext context = scope.ServiceProvider.GetRequiredService<TripDbContext>();
            context.Database.Migrate();
            return app;
        }
    }
}
