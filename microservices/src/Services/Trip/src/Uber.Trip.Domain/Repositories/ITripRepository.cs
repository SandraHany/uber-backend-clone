namespace Uber.Trip.Domain.Repositories;

public interface ITripRepository
{
    Task<Domain.Entities.Trip?> GetByIdAync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Domain.Entities.Trip>> GetActiveByRiderIdAsync(Guid riderId, CancellationToken ct = default);
    void Add(Domain.Entities.Trip trip);
    void Update(Domain.Entities.Trip trip);

}
