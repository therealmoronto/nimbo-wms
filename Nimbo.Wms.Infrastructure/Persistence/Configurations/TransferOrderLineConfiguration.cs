using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Documents.Transfer;
using Nimbo.Wms.Infrastructure.Persistence.Converters;

namespace Nimbo.Wms.Infrastructure.Persistence.Configurations;

public class TransferOrderLineConfiguration : IEntityTypeConfiguration<TransferOrderLine>
{
    public void Configure(EntityTypeBuilder<TransferOrderLine> builder)
    {
        builder.ToTable("transfer_order_lines");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        builder.Property(x => x.ItemId)
            .HasEntityIdConversion()
            .IsRequired();
        
        builder.Property(x => x.DocumentId)
            .HasEntityIdConversion()
            .IsRequired();
        
        builder.OwnsOne(x => x.PlannedQuantity, q =>
        {
            q.Property(p => p.Value)
                .HasColumnName("planned_qty_amount")
                .HasColumnType("numeric(18, 3)")
                .IsRequired();

            q.Property(p => p.Uom)
                .HasColumnName("planned_qty_uom")
                .HasConversion<string>()
                .HasMaxLength(16)
                .IsRequired();

            q.WithOwner();
        });
        
        builder.OwnsOne(x => x.PickedQuantity, q =>
        {
            q.Property(p => p.Value)
                .HasColumnName("picked_qty_amount")
                .HasColumnType("numeric(18, 3)")
                .IsRequired();

            q.Property(p => p.Uom)
                .HasColumnName("picked_qty_uom")
                .HasConversion<string>()
                .HasMaxLength(16)
                .IsRequired();

            q.WithOwner();
        });
        
        builder.OwnsOne(x => x.ReceivedQuantity, q =>
        {
            q.Property(p => p.Value)
                .HasColumnName("received_qty_amount")
                .HasColumnType("numeric(18, 3)")
                .IsRequired();

            q.Property(p => p.Uom)
                .HasColumnName("received_qty_uom")
                .HasConversion<string>()
                .HasMaxLength(16)
                .IsRequired();

            q.WithOwner();
        });

        builder.HasIndex(x => x.ItemId);
    }
}
