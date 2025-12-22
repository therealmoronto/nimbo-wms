using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Domain.Entities;

public interface IEntity<out TId> where TId : struct, IEntityId
{
    TId Id { get; }
}
