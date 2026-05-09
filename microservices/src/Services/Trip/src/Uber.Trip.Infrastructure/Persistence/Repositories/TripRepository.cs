using Microsoft.EntityFrameworkCore;
using Uber.Trip.Domain;
using Uber.Trip.Domain.Repositories;

namespace Uber.Trip.Infrastructure.Persistence.Repositories;

public sealed class TripRepository(TripDbContext dbContext) : ITripRepository
{
    public void Add(Domain.Entities.Trip trip) => dbContext.Trips.Add(trip);
    public void Update(Domain.Entities.Trip trip) => dbContext.Trips.Update(trip);

    public async Task<IReadOnlyList<Domain.Entities.Trip>> GetActiveByRiderIdAsync(Guid riderId, CancellationToken ct = default) =>
    
        await dbContext.Trips.Where(x => x.RiderId == riderId && x.Status == TripStatus.InProgress)
                .ToListAsync(ct);
    

    public async Task<Domain.Entities.Trip?> GetByIdAync(Guid id, CancellationToken ct = default) =>
        await dbContext.Trips
              .FirstOrDefaultAsync(x => x.Id == id,ct);


}
