using Microsoft.AspNetCore.SignalR;

namespace UberMonolith.API;

public class TripHub : Hub
{
    private readonly ILogger<TripHub> _logger;

    public TripHub(ILogger<TripHub> logger)
    {
        _logger = logger;
    }

}
    