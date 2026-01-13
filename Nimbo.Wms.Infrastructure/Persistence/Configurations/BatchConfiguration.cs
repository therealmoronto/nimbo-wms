using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Infrastructure.Persistence.Converters;

namespace Nimbo.Wms.Infrastructure.Persistence.Configurations;

public class BatchConfiguration : IEntityTypeConfiguration<Batch>
{
    public void Configure(EntityTypeBuilder<Batch> builder)
    {
        builder.ToTable("batches"); // nimbo.batches (default schema)

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasEntityIdConversion();

        builder.Property(x => x.ItemId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.BatchNumber)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.SupplierId)
            .HasEntityIdConversion();

        builder.Property(x => x.ManufacturedAt);
        builder.Property(x => x.ExpiryDate);
        builder.Property(x => x.ReceivedAt);

        builder.Property(x => x.Notes)
            .HasMaxLength(512);

        // FK to Item (no navigation in domain)
        builder.HasOne<Item>()
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // FK to Supplier (optional)
        builder.HasOne<Supplier>()
            .WithMany()
            .HasForeignKey(x => x.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        // Batch number must be unique per item
        builder.HasIndex(x => new { x.ItemId, x.BatchNumber })
            .IsUnique();

        builder.HasIndex(x => x.ExpiryDate);
    }
}
