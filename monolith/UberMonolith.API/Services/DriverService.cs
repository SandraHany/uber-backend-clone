using UberMonolith.API.Models.Domains;

namespace UberMonolith.API;

public class DriverService : IDriverService
{

    public Task<List<Ride>> GetRequestedRidesAsync()
    {
        return Task.FromResult(new List<Ride>());
    }
}
