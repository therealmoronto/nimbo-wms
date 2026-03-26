using JetBrains.Annotations;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Domain;

/// <summary>
/// Marker interface for domain events.
/// </summary>
[PublicAPI]
public interface IDomainEvent;

[PublicAPI]
public interface IDomainEventsContainer
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }

    void ClearEvents();
}

[PublicAPI]
public interface IEntity
{
    IEntityId Id { get; }
}

[PublicAPI]
public interface IEntity<out TId> : IEntity
    where TId : struct, IEntityId
{
    new TId Id { get; }
}

[PublicAPI]
public abstract class BaseEntity<TId> : IEntity<TId>, IDomainEventsContainer
    where TId : struct, IEntityId
{
    private readonly List<IDomainEvent> _domainEvents = new();

    IEntityId IEntity.Id => Id;

    public TId Id { get; protected set; }

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearEvents() => _domainEvents.Clear();

    protected void RaiseEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
