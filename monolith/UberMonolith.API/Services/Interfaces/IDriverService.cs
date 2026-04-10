using UberMonolith.API.Models.Domains;

namespace UberMonolith.API;

public interface IDriverService
{
    public Task<List<Ride>> GetRequestedRidesAsync();
}
