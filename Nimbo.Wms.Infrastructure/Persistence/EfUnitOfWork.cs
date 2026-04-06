using System.Text.Json;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Domain;
using Nimbo.Wms.Infrastructure.Persistence.Outbox;

namespace Nimbo.Wms.Infrastructure.Persistence;

internal sealed class EfUnitOfWork(NimboWmsDbContext dbContext) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken ct = default)
    {
        await ConvertDomainEventsToOutboxMessages(ct);
        await dbContext.SaveChangesAsync(ct);
    }

    private async Task ConvertDomainEventsToOutboxMessages(CancellationToken ct = default)
    {
        var entitiesWithEvents = dbContext.ChangeTracker
            .Entries<IDomainEventsContainer>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = entitiesWithEvents
            .SelectMany(x =>
            {
                var events = x.Entity.DomainEvents;
                x.Entity.ClearEvents();
                return events;
            })
            .ToList();

        var outboxMessages = domainEvents.Select(e => new OutboxMessage()
            {
                Type = e.GetType().Name,
                Content = JsonSerializer.Serialize(e, e.GetType()),
            })
            .ToList();

        await dbContext.Set<OutboxMessage>().AddRangeAsync(outboxMessages, ct);
    }
}
