using Microsoft.EntityFrameworkCore;
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

    public static PropertyBuilder<TId?> HasEntityIdConversion<TId>(this PropertyBuilder<TId?> property)
        where TId : struct, IEntityId
    {
        property.HasConversion(new EntityIdConverter<TId>());
        property.Metadata.SetValueComparer(new EntityIdComparer<TId>());
        return property;
    }
    
    public static PropertyBuilder<List<TId>> HasEntityIdListConversion<TId>(
        this PropertyBuilder<List<TId>> property)
        where TId : struct, IEntityId
    {
        property.HasConversion(new EntityIdListConverter<TId>());
        property.Metadata.SetValueComparer(new EntityIdListComparer<TId>());

        // Npgsql will map Guid[] to uuid[]
        property.HasColumnType("uuid[]");

        return property;
    }
}
