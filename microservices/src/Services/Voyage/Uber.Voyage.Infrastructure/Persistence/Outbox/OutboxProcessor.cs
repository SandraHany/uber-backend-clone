using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Uber.Shared;
using Uber.Voyage.Application.Abstractions;
namespace Uber.Voyage.Infrastructure.Persistence.Outbox;

public class OutboxProcessor(ILogger<OutboxProcessor> logger, IServiceScopeFactory scopeFactory) : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromSeconds(5);
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
               await ProcessOutboxMessages(ct);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing outbox messages");

            }
            await Task.Delay(Interval, ct);
        }
    }
    private async Task ProcessOutboxMessages(CancellationToken ct)
    {
        logger.LogInformation("Checking for outbox messages...");
        using var scope = scopeFactory.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<VoyageDbContext>();
        var producer = scope.ServiceProvider.GetRequiredService<IKafkaProducer>();

        var messages = await dbContext.OutboxMessages
            .Where(m => m.ProcessedAt == null)
            .OrderBy(m => m.OccurredAt)
            .Take(50)
            .ToListAsync(ct);
        if (!messages.Any()) return;
        foreach (var message in messages)
        {
            try
            {
                var eventType = Type.GetType(message.Type);

                if (eventType is null)
                {
                    logger.LogWarning(
                        "[OutboxProcessor] Cannot resolve type {Type} for message {Id}",
                        message.Type, message.Id);

                    message.ProcessedAt = DateTime.UtcNow;
                    message.Error = $"Type not found: {message.Type}";
                    continue;
                }
                string topic = "none";
                switch (eventType.Name)
                {
                    case ("VoyageRequestedDomainEvent"):
                        topic = KafkaTopics.VoyageRequested;
                        break;
                }  
               
                var domainEvent = JsonSerializer.Deserialize(message.Payload, eventType)!;
                await producer.PublishAsync(topic, message.Id.ToString(), domainEvent, ct);

                message.ProcessedAt = DateTime.UtcNow;

                logger.LogInformation(
                    "[OutboxProcessor] Published {Type} → {Topic}",
                    eventType.Name, topic);

            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "[OutboxProcessor] Failed to publish message {Id} — will retry",
                    message.Id);
                message.Error = ex.Message;

            }
            //fetch unprocessed messages
            //foreach message, deserialize and publish to message broker
            //mark as processed
        }
        await dbContext.SaveChangesAsync(ct);
    }
    /*private static string ResolveTopicFromType(Type type) => type.Name switch
    {
        "TripRequestedDomainEvent" => Uber.Shared.KafkaTopics.TripRequested,
        "TripAcceptedDomainEvent" => Uber.Shared.KafkaTopics.DriverAccepted,
        "TripStartedDomainEvent" => Uber.Shared.KafkaTopics.TripStarted,
        "TripCompletedDomainEvent" => Uber.Shared.KafkaTopics.TripCompleted,
        "TripCancelledDomainEvent" => Uber.Shared.KafkaTopics.TripCancelled,
        _ => throw new InvalidOperationException($"No topic mapped for: {type.Name}")
    };*/
}


