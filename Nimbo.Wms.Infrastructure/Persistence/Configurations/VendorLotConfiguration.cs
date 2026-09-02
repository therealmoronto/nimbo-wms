using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Infrastructure.Persistence.Converters;

namespace Nimbo.Wms.Infrastructure.Persistence.Configurations;

public class VendorLotConfiguration : IEntityTypeConfiguration<VendorLot>
{
    public void Configure(EntityTypeBuilder<VendorLot> builder)
    {
        builder.ToTable("vendor_lots");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasEntityIdConversion();

        builder.Property(x => x.ItemId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.BatchNumber)
            .HasMaxLength(VendorLot.BatchNumberMaxLength)
            .IsRequired();

        builder.Property(x => x.SupplierId)
            .HasEntityIdConversion();

        builder.Property(x => x.ExpiryDate);

        builder.HasOne<Item>()
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Supplier>()
            .WithMany()
            .HasForeignKey(x => x.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        // Postgres treats NULL as distinct in a plain unique index, so a single index over all four
        // columns would silently allow duplicate VendorLots when SupplierId/ExpiryDate are both null —
        // a very real case (batch-managed item, no supplier, no expiry). Four partial unique indexes,
        // one per null/non-null combination, close that gap without reaching for raw SQL.
        builder.HasIndex(x => new { x.ItemId, x.BatchNumber, x.SupplierId, x.ExpiryDate })
            .IsUnique()
            .HasFilter("\"SupplierId\" IS NOT NULL AND \"ExpiryDate\" IS NOT NULL")
            .HasDatabaseName("ix_vendor_lots_item_batch_supplier_expiry");

        builder.HasIndex(x => new { x.ItemId, x.BatchNumber, x.SupplierId })
            .IsUnique()
            .HasFilter("\"SupplierId\" IS NOT NULL AND \"ExpiryDate\" IS NULL")
            .HasDatabaseName("ix_vendor_lots_item_batch_supplier_only");

        builder.HasIndex(x => new { x.ItemId, x.BatchNumber, x.ExpiryDate })
            .IsUnique()
            .HasFilter("\"SupplierId\" IS NULL AND \"ExpiryDate\" IS NOT NULL")
            .HasDatabaseName("ix_vendor_lots_item_batch_expiry_only");

        builder.HasIndex(x => new { x.ItemId, x.BatchNumber })
            .IsUnique()
            .HasFilter("\"SupplierId\" IS NULL AND \"ExpiryDate\" IS NULL")
            .HasDatabaseName("ix_vendor_lots_item_batch_only");

        builder.HasIndex(x => x.ExpiryDate); // non-unique, FEFO scan support
    }
}
