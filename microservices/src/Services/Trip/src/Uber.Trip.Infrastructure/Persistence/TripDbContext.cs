using Microsoft.EntityFrameworkCore;
namespace Uber.Trip.Infrastructure.Persistence;

public sealed class TripDbContext(DbContextOptions<TripDbContext> options) : DbContext(options)
{
    public DbSet<Domain.Entities.Trip> Trips => Set<Domain.Entities.Trip>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TripDbContext).Assembly);
    

}

