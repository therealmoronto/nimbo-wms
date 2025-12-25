using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistences.Converters;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<TId> HasEntityIdConversion<TId>(this PropertyBuilder<TId> property)
        where TId : struct, IEntityId
    {
        property.HasConversion(new EntityIdConverter<TId>());
        property.Metadata.SetValueComparer(new EntityIdComparer<TId>());
        return property;
    }
}
