using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Domain;

public interface IEntity
{
    IEntityId Id { get; }
}

public interface IEntity<out TId> : IEntity
    where TId : struct, IEntityId
{
    new TId Id { get; }
}

public abstract class BaseEntity<TId> : IEntity<TId>
    where TId : struct, IEntityId
{
    IEntityId IEntity.Id => Id;

    public TId Id { get; protected set; }
}
