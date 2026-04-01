using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Entities.Documents.Common;
using Nimbo.Wms.Domain.Entities.Documents.CycleCount;
using Nimbo.Wms.Infrastructure.Persistence.Converters;

namespace Nimbo.Wms.Infrastructure.Persistence.Configurations;

public class CycleCountDocumentConfiguration : IEntityTypeConfiguration<CycleCountDocument>
{
    public void Configure(EntityTypeBuilder<CycleCountDocument> builder)
    {
        builder.ToTable("cycle_count_documents");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasEntityIdConversion();

        builder.Property(x => x.WarehouseId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.Code)
            .HasMaxLength(IDocument.CodeMaxLength)
            .IsRequired();

        builder.Property(x => x.Title)
            .HasMaxLength(IDocument.TitleMaxLength)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.Property(x => x.PostedAt);

        builder.Property(x => x.Version).IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(IDocument.NotesMaxLength);

        builder.HasMany(x => x.Lines)
            .WithOne()
            .HasForeignKey(x => x.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.Code).IsUnique();
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.WarehouseId);
        builder.HasIndex(x => x.CreatedAt);
    }
}
