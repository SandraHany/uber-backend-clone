using UberMonolith.API.Models.Domains;

namespace UberMonolith.API;

public interface IDriverRepository
{
    public Task<List<Driver>> GetNearbyDrivers();
}
