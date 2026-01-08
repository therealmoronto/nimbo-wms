using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Documents.Audit;
using Nimbo.Wms.Infrastructure.Persistence.Converters;

namespace Nimbo.Wms.Infrastructure.Persistence.Configurations;

public class InventoryCountLineConfiguration : IEntityTypeConfiguration<InventoryCountLine>
{
    public void Configure(EntityTypeBuilder<InventoryCountLine> builder)
    {
        builder.ToTable("inventory_count_lines");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.DocumentId)
            .HasEntityIdConversion()
            .IsRequired();
        
        builder.Property(x => x.ItemId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.LocationId)
            .HasEntityIdConversion()
            .IsRequired();

        // SystemQuantity (required)
        builder.OwnsOne(x => x.SystemQuantity, q =>
        {
            q.Property(p => p.Value)
                .HasColumnName("system_qty_amount")
                .HasColumnType("numeric(18, 3)")
                .IsRequired();

            q.Property(p => p.Uom)
                .HasColumnName("system_qty_uom")
                .HasConversion<string>()
                .HasMaxLength(16)
                .IsRequired();

            q.WithOwner();
        });

        builder.Navigation(x => x.SystemQuantity).IsRequired();

        // CountedQuantity (optional)
        builder.OwnsOne(x => x.CountedQuantity, q =>
        {
            q.Property(p => p.Value)
                .HasColumnName("counted_qty_amount")
                .HasColumnType("numeric(18, 3)");

            q.Property(p => p.Uom)
                .HasColumnName("counted_qty_uom")
                .HasConversion<string>()
                .HasMaxLength(16);

            q.WithOwner();
        });

        builder.Navigation(x => x.CountedQuantity).IsRequired(false);

        builder.HasIndex(x => x.ItemId);
        builder.HasIndex(x => x.LocationId);
    }
}
