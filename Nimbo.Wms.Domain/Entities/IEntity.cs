using JetBrains.Annotations;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Domain.Entities;

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
public abstract class BaseEntity<TId> : IEntity<TId>
    where TId : struct, IEntityId
{
    private readonly List<IDomainEvent> _domainEvents = new();

    IEntityId IEntity.Id => Id;

    public TId Id { get; protected set; }
}
