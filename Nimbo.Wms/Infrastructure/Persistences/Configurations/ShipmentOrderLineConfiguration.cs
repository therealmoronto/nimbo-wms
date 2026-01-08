using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Documents.Outbound;
using Nimbo.Wms.Infrastructure.Persistences.Converters;

namespace Nimbo.Wms.Infrastructure.Persistences.Configurations;

public class ShipmentOrderLineConfiguration : IEntityTypeConfiguration<ShipmentOrderLine>
{
    public void Configure(EntityTypeBuilder<ShipmentOrderLine> builder)
    {
        builder.ToTable("shipment_order_lines");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        builder.Property(x => x.ShipmentOrderId)
            .HasEntityIdConversion()
            .IsRequired();
        
        builder.Property(x => x.ItemId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.OrderedQuantity)
            .HasColumnType("numeric(8)")
            .IsRequired();

        builder.Property(x => x.ReservedQuantity)
            .HasColumnType("numeric(8)")
            .IsRequired();

        builder.Property(x => x.PickedQuantity)
            .HasColumnType("numeric(8)")
            .IsRequired();

        builder.Property(x => x.UomCode)
            .HasMaxLength(16)
            .IsRequired();
        
        builder.HasIndex(x => x.ItemId);
    }
}
