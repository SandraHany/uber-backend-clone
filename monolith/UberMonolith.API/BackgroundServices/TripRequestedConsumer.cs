
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;

namespace UberMonolith.API;

public class TripRequestedConsumer : BackgroundService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly ILogger<TripRequestedConsumer> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public TripRequestedConsumer(IServiceScopeFactory scopeFactory, IConsumer<string, string> consumer, ILogger<TripRequestedConsumer> logger)
    {
        _consumer = consumer;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe("trip-requested"); 
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {

                _logger.LogInformation("Consuming messages from 'trip-requested' topic...");
                var result = _consumer.Consume(stoppingToken);
                            if (result.IsPartitionEOF)
                {
                    _logger.LogInformation("End of partition reached");
                    continue;
                }

                if (result.Message == null)
                {
                    _logger.LogError("Consumed message is null");
                    continue;
                }

                _logger.LogInformation($"Consumed message '{result.Message.Value}' from '{result.TopicPartition}'");
                var rideRequestedEvent = JsonSerializer.Deserialize<RideRequestedEvent>(result.Message.Value);
                using var scope = _scopeFactory.CreateScope();
                var rideService = scope.ServiceProvider.GetRequiredService<IRideService>();
                var nearbyDrivers = await rideService.GetNearbyDrivers(double.Parse(rideRequestedEvent.PickupLatitude), double.Parse(rideRequestedEvent.PickupLongitude), 3);
                _logger.LogInformation($"Found {nearbyDrivers.Count} nearby drivers for ride request");
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<TripHub>>();
                /*foreach (var driver in nearbyDrivers)
                {
                    var notification = new RideRequestedEvent
                    {
                        RiderId = rideRequestedEvent.RiderId,
                        DriverId = driver.Id,
                        RequestedAt = DateTime.UtcNow,
                        PickupLatitude = rideRequestedEvent.PickupLatitude,
                        PickupLongitude = rideRequestedEvent.PickupLongitude,
                        DropoffLatitude = rideRequestedEvent.DropoffLatitude,
                        DropoffLongitude = rideRequestedEvent.DropoffLongitude
                    };
                    _logger.LogInformation($"Notifying driver {driver.Id} of new ride request");
                    await hubContext.Clients.User(driver.Id.ToString()).SendAsync("ReceiveRideRequest", notification);
                }*/
                await hubContext.Clients.All.SendAsync("ReceiveRideRequest", rideRequestedEvent);
                _consumer.Commit(result);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("TripRequestedConsumer is stopping...");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consuming messages from 'trip-requested' topic");
            }

        }
    }
}
