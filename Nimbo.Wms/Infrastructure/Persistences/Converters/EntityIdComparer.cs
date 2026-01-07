using Microsoft.EntityFrameworkCore.ChangeTracking;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistences.Converters;

public sealed class EntityIdComparer<TId> : ValueComparer<TId>
    where TId : struct, IEntityId
{
    public EntityIdComparer()
        : base(
            (a, b) => a.Value.Equals(b.Value),
            v => v.Value.GetHashCode(),
            v => v) { }
}
