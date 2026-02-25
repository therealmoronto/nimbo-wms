using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Entities.Documents.CycleCount;
using Nimbo.Wms.Infrastructure.Persistence.Converters;

namespace Nimbo.Wms.Infrastructure.Persistence.Configurations;

public class CycleCountDocumentLineConfiguration : IEntityTypeConfiguration<CycleCountDocumentLine>
{
    public void Configure(EntityTypeBuilder<CycleCountDocumentLine> builder)
    {
        builder.ToTable("cycle_count_document_lines");

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

        builder.OwnsOne(
            x => x.Quantity,
            q =>
            {
                q.Property(p => p.Value)
                    .HasColumnName("expected_quantity_amount")
                    .HasColumnType("numeric(18, 3)")
                    .IsRequired();

                q.Property(p => p.Uom)
                    .HasColumnName("expected_quantity_uom")
                    .HasConversion<string>()
                    .HasMaxLength(16)
                    .IsRequired();

                q.WithOwner();
            });

        builder.Navigation(x => x.Quantity).IsRequired();

        builder.OwnsOne(
            x => x.ActualQuantity,
            q =>
            {
                q.Property(p => p.Value)
                    .HasColumnName("actual_quantity_amount")
                    .HasColumnType("numeric(18, 3)");

                q.Property(p => p.Uom)
                    .HasColumnName("actual_quantity_uom")
                    .HasConversion<string>()
                    .HasMaxLength(16);

                q.WithOwner();
            });

        // usually optional; keep it explicit
        builder.Navigation(x => x.ActualQuantity).IsRequired(false);

        builder.HasIndex(x => x.DocumentId);
        builder.HasIndex(x => new { x.DocumentId, x.ItemId, x.LocationId }).IsUnique();
    }
}
