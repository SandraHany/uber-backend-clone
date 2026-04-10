
using Confluent.Kafka;

namespace UberMonolith.API;

public class TripRequestedConsumer : BackgroundService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly ILogger<TripRequestedConsumer> _logger;

    public TripRequestedConsumer(IConsumer<string, string> consumer, ILogger<TripRequestedConsumer> logger)
    {
        _consumer = consumer;
        _logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe("trip-requested"); 
        while (!stoppingToken.IsCancellationRequested)
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
        }
    }
}
