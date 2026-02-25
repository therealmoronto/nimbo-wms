using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Entities.Documents.Adjustment;
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

        builder.Property(x => x.Notes)
            .HasMaxLength(512);
        
        builder.Ignore(x => x.Quantity);
        
        builder.OwnsOne(x => x.Delta, d =>
        {
            d.Property(p => p.Value)
                .HasColumnName("delta_amount")
                .HasColumnType("numeric(18, 3)")
                .IsRequired();

            d.Property(p => p.Uom)
                .HasColumnName("delta_uom")
                .HasConversion<string>()
                .HasMaxLength(16)
                .IsRequired();

            d.WithOwner();
        });
        
        builder.Navigation(x => x.Delta).IsRequired();

        builder.HasIndex(x => x.DocumentId);
        builder.HasIndex(x => new { x.DocumentId, x.ItemId, x.LocationId });
    }
}
