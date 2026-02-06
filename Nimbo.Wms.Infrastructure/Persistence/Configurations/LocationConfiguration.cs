using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Infrastructure.Persistence.Converters;

namespace Nimbo.Wms.Infrastructure.Persistence.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("locations");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasEntityIdConversion();
        
        builder.Property(x => x.WarehouseId)
            .HasEntityIdConversion()
            .IsRequired();
        
        builder.Property(x => x.ZoneId)
            .HasEntityIdConversion()
            .IsRequired();
        
        builder.Property(x => x.Code)
            .HasMaxLength(32)
            .IsRequired();
        
        builder.Property(x => x.Aisle)
            .HasMaxLength(16);
        
        builder.Property(x => x.Rack)
            .HasMaxLength(16);
        
        builder.Property(x => x.Level)
            .HasMaxLength(16);
        
        builder.Property(x => x.Position)
            .HasMaxLength(16);
        
        builder.Property(x => x.Type)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.MaxWeightKg);
        
        builder.Property(x => x.MaxVolumeM3);
        
        builder.Property(x => x.IsSingleItemOnly)
            .IsRequired();
        
        builder.Property(x => x.IsPickingLocation)
            .IsRequired();
        
        builder.Property(x => x.IsReceivingLocation)
            .IsRequired();
        
        builder.Property(x => x.IsShippingLocation)
            .IsRequired();
        
        builder.Property(x => x.IsActive)
            .IsRequired();
        
        builder.Property(x => x.IsBlocked)
            .IsRequired();

        // Unique per warehouse: you can't have two locations with the same code in same warehouse
        builder.HasIndex(x => new { x.WarehouseId, x.Code }).IsUnique();
        builder.HasIndex(x => x.ZoneId);
        builder.HasIndex(x => x.IsActive);
    }
}
