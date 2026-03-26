using System.Text.Json;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Domain;
using Nimbo.Wms.Infrastructure.Persistence.Outbox;

namespace Nimbo.Wms.Infrastructure.Persistence;

internal sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly NimboWmsDbContext _dbContext;

    public EfUnitOfWork(NimboWmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task CommitAsync(CancellationToken ct = default)
    {
        return _dbContext.SaveChangesAsync(ct);
    }

    private void ConvertDomainEventsToOutboxMessages()
    {
        var entitiesWithEvents = _dbContext.ChangeTracker
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

        _dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
    }
}
