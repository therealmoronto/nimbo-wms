using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Domain;

public interface IEntity<out TId> where TId : struct, IEntityId
{
    TId Id { get; }
}
