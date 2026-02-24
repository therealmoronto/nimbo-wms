using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Entities.Documents.Adjustment;
using Nimbo.Wms.Infrastructure.Persistence.Converters;

namespace Nimbo.Wms.Infrastructure.Persistence.Configurations;

public class AdjustmentDocumentConfiguration : IEntityTypeConfiguration<AdjustmentDocument>
{
    public void Configure(EntityTypeBuilder<AdjustmentDocument> builder)
    {
        builder.ToTable("adjustment_documents");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasEntityIdConversion()
            .ValueGeneratedNever();

        builder.Property(x => x.WarehouseId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.Code)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.Title)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.Property(x => x.PostedAt);

        builder.Property(x => x.Version).IsRequired();

        builder.Property(x => x.Notes).HasMaxLength(512);

        builder.Property(x => x.ReasonCode)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(x => x.ReasonText)
            .HasMaxLength(256);

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
