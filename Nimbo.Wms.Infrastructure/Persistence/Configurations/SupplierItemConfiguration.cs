using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Infrastructure.Persistence.Converters;

namespace Nimbo.Wms.Infrastructure.Persistence.Configurations;

public class SupplierItemConfiguration : IEntityTypeConfiguration<SupplierItem>
{
    public void Configure(EntityTypeBuilder<SupplierItem> builder)
    {
        builder.ToTable("supplier_items");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasEntityIdConversion();
        
        builder.Property(x => x.SupplierId)
            .HasEntityIdConversion()
            .IsRequired();
        
        builder.Property(x => x.ItemId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.SupplierSku)
            .HasMaxLength(128);
        
        builder.Property(x => x.SupplierBarcode)
            .HasMaxLength(64);

        builder.Property(x => x.DefaultPurchasePrice);
        builder.Property(x => x.PurchaseUomCode);
        builder.Property(x => x.UnitsPerPurchaseUom);
        builder.Property(x => x.LeadTimeDays);
        builder.Property(x => x.MinOrderQty);

        builder.Property(x => x.IsPreferred)
            .IsRequired();

        builder.HasOne<Item>()
            .WithMany()
            .HasForeignKey(x => x.ItemId);

        builder.HasIndex(x => new { x.SupplierId, x.ItemId });
    }
}
