using System.Text.Json;
using Confluent.Kafka;
using UberMonolith.API.Models.Domains;
using UberMonolith.API.Models.DTOs;
using UberMonolith.API.Repositories;

namespace UberMonolith.API;

public class RideService : IRideService
{
    private readonly IRideRepository _rideRepository;
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<RideService> _logger;
    public RideService(IRideRepository rideRepository, IProducer<string, string> producer, ILogger<RideService> logger)
    {
        _rideRepository = rideRepository;
        _producer = producer;
        _logger = logger;
    }
    public async Task<Ride> RequestNewRide(Ride request)
    {
        var message = new Message<string, string>
        {
            Value = JsonSerializer.Serialize(request)
        };
        try
        {
            var deliveryReport = await _producer.ProduceAsync("trip-requested", message);
            if (deliveryReport.Status == PersistenceStatus.NotPersisted)
            {
                _logger.LogError($"Failed to produce message: {deliveryReport.Message.Value}");
            }
            else
            {
                _logger.LogInformation($"Message produced to topic {deliveryReport.Topic} partition {deliveryReport.Partition} offset {deliveryReport.Offset}");
            }
        }
        catch (ProduceException<string, string> ex)
        {
            _logger.LogError($"An error occurred while producing the message: {ex.Error.Reason}");
        }
        var response = await _rideRepository.RequestNewRide(request);
        return response;
    }
    public Task<List<NearbyDriverDto>> GetNearbyDrivers(double latitude, double longitude, double radiusKm)
    {
        return _rideRepository.GetNearbyDrivers(latitude, longitude, radiusKm);
    }

}
