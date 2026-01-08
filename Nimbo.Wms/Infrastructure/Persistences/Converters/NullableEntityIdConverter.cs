using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistences.Converters;

public sealed class NullableEntityIdConverter<TId> : ValueConverter<TId?, Guid?>
    where TId : struct, IEntityId
{
    public NullableEntityIdConverter()
        : base(
            v => v.HasValue ? v.Value.Value : (Guid?)null,
            v => v.HasValue ? (TId)Activator.CreateInstance(typeof(TId), v.Value)! : (TId?)null)
    { }
}
