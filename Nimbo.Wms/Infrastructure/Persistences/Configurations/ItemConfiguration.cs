using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Infrastructure.Persistences.Converters;

namespace Nimbo.Wms.Infrastructure.Persistences.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("items");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasEntityIdConversion();
        
        builder.Property(x => x.Name)
            .HasMaxLength(512)
            .IsRequired();
        
        builder.Property(x => x.InternalSku)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.Barcode)
            .HasMaxLength(64);
        
        builder.Property(x => x.BaseUomCode)
            .IsRequired();

        builder.Property(x => x.Manufacturer)
            .HasMaxLength(128);
        
        builder.Property(x => x.WeightKg);
        builder.Property(x => x.VolumeM3);
        
        builder.HasIndex(x => x.InternalSku).IsUnique();
    }
}
