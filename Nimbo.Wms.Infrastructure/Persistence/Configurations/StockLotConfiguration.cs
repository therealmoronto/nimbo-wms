using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Entities.Documents.Receiving;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Infrastructure.Persistence.Converters;

namespace Nimbo.Wms.Infrastructure.Persistence.Configurations;

public class StockLotConfiguration : IEntityTypeConfiguration<StockLot>
{
    public void Configure(EntityTypeBuilder<StockLot> builder)
    {
        builder.ToTable("stock_lots");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasEntityIdConversion();

        builder.Property(x => x.ItemId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.ReceivingDocumentId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.ReceivedAt)
            .IsRequired();

        builder.Property(x => x.VendorLotId)
            .HasEntityIdConversion();

        builder.Property(x => x.UnitCost)
            .HasColumnType("numeric(18, 4)");

        builder.HasOne<Item>()
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<ReceivingDocument>()
            .WithMany()
            .HasForeignKey(x => x.ReceivingDocumentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<VendorLot>()
            .WithMany()
            .HasForeignKey(x => x.VendorLotId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.ReceivingDocumentId);
        builder.HasIndex(x => new { x.ItemId, x.ReceivedAt }); // FIFO scan
        builder.HasIndex(x => x.VendorLotId);
    }
}
