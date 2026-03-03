using Microsoft.EntityFrameworkCore;
using UberMonolith.Domain;
namespace UberMonolith.Infrastructure;

public class AppDbContext :
 DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }