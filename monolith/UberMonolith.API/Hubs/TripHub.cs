using Microsoft.AspNetCore.SignalR;

namespace UberMonolith.API;

public class TripHub : Hub
{
    private readonly ILogger<TripHub> _logger;

    public TripHub(ILogger<TripHub> logger)
    {
        _logger = logger;
    }
    public void AcceptTrip()
    {
            _logger.LogInformation($"Driver {Context.UserIdentifier} accepted the trip");
    }

    public void StartTrip()
    {
            _logger.LogInformation($"Driver {Context.UserIdentifier} started the trip");
    }

}
    