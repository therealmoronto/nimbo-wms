using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Infrastructure.Persistence.Converters;

namespace Nimbo.Wms.Infrastructure.Persistence.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("items");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasEntityIdConversion();
        
        builder.Property(x => x.Name)
            .HasMaxLength(Item.NameMaxLength)
            .IsRequired();
        
        builder.Property(x => x.InternalSku)
            .HasMaxLength(Item.InternalSkuMaxLength)
            .IsRequired();

        builder.Property(x => x.Barcode)
            .HasMaxLength(Item.BarcodeMaxLength)
            .IsRequired();
        
        builder.Property(x => x.BaseUomCode)
            .IsRequired();

        builder.Property(x => x.Manufacturer)
            .HasMaxLength(Item.ManufacturerMaxLength);

        builder.Property(x => x.WeightKg);
        builder.Property(x => x.VolumeM3);
        
        builder.HasIndex(x => x.InternalSku).IsUnique();
    }
}
