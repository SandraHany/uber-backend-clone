using UberMonolith.API.Models.Domains;

namespace UberMonolith.API.Repositories;

public interface IDriverRepository
{
    public Task UpdateDriverLocation(Guid driverId, double latitude, double longitude);
    public Task<List<Driver>> GetDriversByIdsAsync(List<Guid> driverIds);
    public Task CreateDriverAsync(Driver driver);
    public Task<Driver?> GetDriverByIdAsync(Guid driverId);
}
