using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Entities.Documents.Adjustment;
using Nimbo.Wms.Domain.Entities.Documents.Common;
using Nimbo.Wms.Infrastructure.Persistence.Converters;

namespace Nimbo.Wms.Infrastructure.Persistence.Configurations;

public class AdjustmentDocumentLineConfiguration : IEntityTypeConfiguration<AdjustmentDocumentLine>
{
    public void Configure(EntityTypeBuilder<AdjustmentDocumentLine> builder)
    {
        builder.ToTable("adjustment_document_lines");
        
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

        builder.Ignore(x => x.Quantity);

        builder.ComplexProperty(
            x => x.Delta,
            b =>
            {
                b.Property(p => p.Value)
                    .HasColumnName("delta_amount")
                    .HasColumnType("numeric(18, 3)")
                    .IsRequired();

                b.Property(p => p.Uom)
                    .HasColumnName("delta_uom")
                    .HasConversion<string>()
                    .HasMaxLength(16)
                    .IsRequired();
            });

        builder.Property(x => x.Notes)
            .HasMaxLength(IDocumentLine.NotesMaxLength);

        builder.HasIndex(x => x.DocumentId);
        builder.HasIndex(x => new { x.DocumentId, x.ItemId, x.LocationId });
    }
}
