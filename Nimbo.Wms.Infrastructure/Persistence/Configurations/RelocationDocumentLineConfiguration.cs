using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Entities.Documents.Relocation;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Infrastructure.Persistence.Converters;

namespace Nimbo.Wms.Infrastructure.Persistence.Configurations;

public class RelocationDocumentLineConfiguration : IEntityTypeConfiguration<RelocationDocumentLine>
{
    public void Configure(EntityTypeBuilder<RelocationDocumentLine> builder)
    {
        builder.ToTable("relocation_document_lines");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.DocumentId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.ItemId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.From)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.To)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(512);

        builder.OwnsOne(
            x => x.Quantity,
            q =>
            {
                q.Property(p => p.Value)
                    .HasColumnName("quantity_amount")
                    .HasColumnType("numeric(18, 3)")
                    .IsRequired();

                q.Property(p => p.Uom)
                    .HasColumnName("quantity_uom")
                    .HasConversion<string>()
                    .HasMaxLength(16)
                    .IsRequired();

                q.WithOwner();
            });

        builder.Navigation(x => x.Quantity).IsRequired();

        builder.HasOne<Item>()
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Location>()
            .WithMany()
            .HasForeignKey(x => x.From)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Location>()
            .WithMany()
            .HasForeignKey(x => x.To)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.DocumentId);
        builder.HasIndex(x => new { x.DocumentId, x.ItemId });
        builder.HasIndex(x => new { x.DocumentId, x.ItemId, x.From, x.To });
    }
}
