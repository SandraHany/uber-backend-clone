using Microsoft.EntityFrameworkCore;
using UberMonolith.API.Models.Domains;

namespace UberMonolith.API.Infrastructure.Data;

public class AppDbContext :
 DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Rider> Riders { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Ride> Rides { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
    }



