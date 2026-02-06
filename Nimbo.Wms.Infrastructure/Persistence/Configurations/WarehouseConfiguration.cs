using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Entities.WarehouseData;
using Nimbo.Wms.Infrastructure.Persistence.Converters;

namespace Nimbo.Wms.Infrastructure.Persistence.Configurations;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.ToTable("warehouses");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasEntityIdConversion();
        
        builder.Property(x => x.Code)
            .HasMaxLength(32)
            .IsRequired();
        
        builder.Property(x => x.Name)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.Address)
            .HasMaxLength(256);

        builder.Property(x => x.Description)
            .HasMaxLength(512);

        builder.Property(x => x.IsActive)
            .IsRequired();
        
        // Zones collection (backing field)
        builder.HasMany(x => x.Zones)
            .WithOne()
            .HasForeignKey(z => z.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Zones)
            .HasField("_zones")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // Locations collection (backing field)
        builder.HasMany(x => x.Locations)
            .WithOne()
            .HasForeignKey(l => l.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Locations)
            .HasField("_locations")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        
        builder.HasIndex(x => x.Code).IsUnique();
        builder.HasIndex(x => x.IsActive);
    }
}
