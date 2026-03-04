using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Entities.Ledger;
using Nimbo.Wms.Infrastructure.Persistence.Converters;

namespace Nimbo.Wms.Infrastructure.Persistence.Configurations;

public class StockLedgerEntryConfiguration : IEntityTypeConfiguration<StockLedgerEntry>
{
    public void Configure(EntityTypeBuilder<StockLedgerEntry> builder)
    {
        builder.ToTable("stock_ledger_entries");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasEntityIdConversion();

        builder.Property(x => x.ItemId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.LocationId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.WarehouseId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.InventoryItemId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.SourceDocumentId)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.SourceDocumentLineId)
            .ValueGeneratedNever()
            .IsRequired();

        builder.OwnsOne(
            x => x.QuantityDelta,
            q =>
            {
                q.Property(p => p.Value)
                    .HasColumnName("quantity_delta_amount")
                    .HasColumnType("numeric(18, 3)")
                    .IsRequired();

                q.Property(p => p.Uom)
                    .HasColumnName("quantity_delta_uom")
                    .HasConversion<string>()
                    .HasMaxLength(16)
                    .IsRequired();
            });
        builder.Navigation(x => x.QuantityDelta).IsRequired();

        builder.OwnsOne(
            x => x.BalanceAfter,
            q =>
            {
                q.Property(p => p.Value)
                    .HasColumnName("balance_after_amount")
                    .HasColumnType("numeric(18, 3)")
                    .IsRequired();

                q.Property(p => p.Uom)
                    .HasColumnName("balance_after_uom")
                    .HasConversion<string>()
                    .HasMaxLength(16)
                    .IsRequired();
            });
        builder.Navigation(x => x.BalanceAfter).IsRequired();

        builder.Property(x => x.TransactionType)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.OccurredAt)
            .IsRequired();

        builder.HasIndex(x => x.SourceDocumentId);
        builder.HasIndex(x => new { x.InventoryItemId, x.OccurredAt });
        builder.HasIndex(x => new { x.ItemId, x.LocationId });
    }
}
