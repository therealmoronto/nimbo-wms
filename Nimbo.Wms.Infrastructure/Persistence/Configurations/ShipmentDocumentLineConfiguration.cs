using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Entities.Documents.Shipment;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Infrastructure.Persistence.Converters;

namespace Nimbo.Wms.Infrastructure.Persistence.Configurations;

public class ShipmentDocumentLineConfiguration : IEntityTypeConfiguration<ShipmentDocumentLine>
{
    public void Configure(EntityTypeBuilder<ShipmentDocumentLine> builder)
    {
        builder.ToTable("shipment_document_lines");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.DocumentId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.ItemId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(512);

        // base.Quantity == RequestedQuantity
        builder.OwnsOne(
            x => x.Quantity,
            q =>
            {
                q.Property(p => p.Value)
                    .HasColumnName("requested_quantity_amount")
                    .HasColumnType("numeric(18, 3)")
                    .IsRequired();

                q.Property(p => p.Uom)
                    .HasColumnName("requested_quantity_uom")
                    .HasConversion<string>()
                    .HasMaxLength(16)
                    .IsRequired();

                q.WithOwner();
            });

        // ShippedQuantity is nullable
        builder.OwnsOne(
            x => x.ShippedQuantity,
            q =>
            {
                q.Property(p => p.Value)
                    .HasColumnName("shipped_quantity_amount")
                    .HasColumnType("numeric(18, 3)");

                q.Property(p => p.Uom)
                    .HasColumnName("shipped_quantity_uom")
                    .HasConversion<string>()
                    .HasMaxLength(16);

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
