using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Documents.Audit;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Infrastructure.Persistences.Converters;

namespace Nimbo.Wms.Infrastructure.Persistences.Configurations;

public sealed class InventoryCountConfiguration : IEntityTypeConfiguration<InventoryCount>
{
    public void Configure(EntityTypeBuilder<InventoryCount> builder)
    {
        builder.ToTable("inventory_counts");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.WarehouseId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.ZoneId)
            .HasEntityIdConversion();
        
        builder.Property(x => x.CreatedAt)
            .IsRequired();
        
        builder.Property(x => x.Status)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.ExternalReference)
            .HasMaxLength(512);

        builder.Property(x => x.StartedAt);
        builder.Property(x => x.ClosedAt);
        
        // LocationScope is IReadOnlyCollection<LocationId> exposed via backing field _locationScope
        builder.Property<List<LocationId>>("_locationScope")
            .HasColumnName("location_scope")
            .HasEntityIdListConversion()   // uuid[]
            .IsRequired();
        
        // Ensure EF uses field access (so it doesn't try to set LocationScope)
        builder.Navigation(x => x.LocationScope)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
