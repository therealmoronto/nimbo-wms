using Microsoft.EntityFrameworkCore.ChangeTracking;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Converters;

public sealed class NullableEntityIdComparer<TId> : ValueComparer<TId?>
    where TId : struct, IEntityId
{
    public NullableEntityIdComparer()
        : base(
            (a, b) => (a.HasValue == b.HasValue) && (!a.HasValue || a.Value.Value == b!.Value.Value),
            v => v.HasValue ? v.Value.Value.GetHashCode() : 0,
            v => v)
    { }
}
