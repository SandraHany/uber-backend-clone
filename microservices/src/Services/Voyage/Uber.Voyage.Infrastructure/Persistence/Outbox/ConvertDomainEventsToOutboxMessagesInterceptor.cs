using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Uber.Shared.Primitives;

namespace Uber.Voyage.Infrastructure.Persistence.Outbox;

public class ConvertDomainEventsToOutboxMessage(ILogger<ConvertDomainEventsToOutboxMessage> logger) : SaveChangesInterceptor
{

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,CancellationToken ct)
    {
        logger.LogInformation("Converting domain events to outbox messages.");
        var dbContext = eventData.Context;
        if (dbContext == null) return base.SavingChangesAsync(eventData, result, ct);
        var outboxMessages = dbContext.ChangeTracker.Entries<AggregateRoot>()
            .Where(e => e.Entity.DomainEvents != null && e.Entity.DomainEvents.Any())
            .SelectMany(e => e.Entity.DomainEvents)
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredAt = DateTime.UtcNow,
                Type = domainEvent.GetType().AssemblyQualifiedName!,
                Payload = JsonSerializer.Serialize(domainEvent, domainEvent.GetType())
            })
            .ToList();
        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
        foreach (var aggregate in dbContext.ChangeTracker.Entries<AggregateRoot>())
            aggregate.Entity.ClearDomainEvents();
        return base.SavingChangesAsync(eventData, result,ct);
    }
}