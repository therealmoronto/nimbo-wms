using JetBrains.Annotations;
using Nimbo.Wms.Domain.Identification;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Domain;

/// <summary>
/// Marker interface for domain events.
/// </summary>
[PublicAPI]
public interface IDomainEvent
{
    Guid Id { get; }

    Guid AggregateId { get; }

    DateTime OccurredAt { get; }
}

[PublicAPI]
public interface IAggregateRoot
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }

    void ClearEvents();
}

[PublicAPI]
public abstract class AggregateRoot<TId> : IEntity<TId>, IAggregateRoot
    where TId : struct, IEntityId
{
    private readonly List<IDomainEvent> _domainEvents = new();

    IEntityId IEntity.Id => Id;

    public abstract TId Id { get; }

    [MapperIgnore]
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearEvents() => _domainEvents.Clear();

    protected void RaiseEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
