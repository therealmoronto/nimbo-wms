using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Entities.WarehouseData;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Infrastructure.Persistences.Converters;

namespace Nimbo.Wms.Infrastructure.Persistences.Configurations;

public class ZoneConfiguration : IEntityTypeConfiguration<Zone>
{
    public void Configure(EntityTypeBuilder<Zone> builder)
    {
        builder.ToTable("zones");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasEntityIdConversion();
        
        builder.Property(x => x.WarehouseId)
            .HasEntityIdConversion();

        builder.Property(x => x.Code)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.Type)
            .HasDefaultValue(ZoneType.Storage);

        builder.Property(x => x.IsDamagedArea)
            .HasDefaultValue(false);

        builder.Property(x => x.IsQuarantine)
            .HasDefaultValue(false);

        builder.Property(x => x.MaxVolumeM3);
        builder.Property(x => x.MaxWeightKg);
        
        builder.HasOne<Warehouse>()
            .WithMany()
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        // Uniqueness: zone codes unique per warehouse
        builder.HasIndex(x => new { x.WarehouseId, x.Code }).IsUnique();
    }
}
