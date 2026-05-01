using JetBrains.Annotations;

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
