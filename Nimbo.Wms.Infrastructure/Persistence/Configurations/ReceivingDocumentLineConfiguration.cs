using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Entities.Documents.Receiving;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Infrastructure.Persistence.Converters;

namespace Nimbo.Wms.Infrastructure.Persistence.Configurations;

public class ReceivingDocumentLineConfiguration : IEntityTypeConfiguration<ReceivingDocumentLine>
{
    public void Configure(EntityTypeBuilder<ReceivingDocumentLine> builder)
    {
        builder.ToTable("receiving_document_lines");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.DocumentId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.ItemId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.ToLocationId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(512);

        builder.OwnsOne(
            x => x.Quantity,
            q =>
            {
                q.Property(x => x.Value)
                    .HasColumnName("quantity_amount")
                    .HasColumnType("numeric(18, 3)")
                    .IsRequired();

                q.Property(x => x.Uom)
                    .HasColumnName("quantity_uom")
                    .HasConversion<string>()
                    .HasMaxLength(16)
                    .IsRequired();

                q.WithOwner();
            });

        builder.Navigation(x => x.Quantity).IsRequired();

        builder.OwnsOne(
            x => x.ExpectedQuantity,
            q =>
            {
                q.Property(x => x.Value)
                    .HasColumnName("expected_quantity_amount")
                    .HasColumnType("numeric(18, 3)")
                    .IsRequired();

                q.Property(x => x.Uom)
                    .HasColumnName("expected_quantity_uom")
                    .HasConversion<string>()
                    .HasMaxLength(16)
                    .IsRequired();

                q.WithOwner();
            });

        builder.HasOne<Item>()
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.DocumentId);
        builder.HasIndex(x => new { x.DocumentId, x.ItemId });
    }
}
