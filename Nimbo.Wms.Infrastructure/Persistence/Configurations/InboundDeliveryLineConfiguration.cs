using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Documents.Inbound;
using Nimbo.Wms.Infrastructure.Persistence.Converters;

namespace Nimbo.Wms.Infrastructure.Persistence.Configurations;

public class InboundDeliveryLineConfiguration : IEntityTypeConfiguration<InboundDeliveryLine>
{
    public void Configure(EntityTypeBuilder<InboundDeliveryLine> builder)
    {
        builder.ToTable("inbound_delivery_lines");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        builder.Property(x => x.ItemId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.DocumentId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.ExpectedQuantity)
            .HasColumnType("numeric(8)")
            .IsRequired();

        builder.Property(x => x.ReceivedQuantity)
            .HasColumnType("numeric(8)");
        
        builder.Property(x => x.Uom)
            .HasMaxLength(16)
            .IsRequired();
        
        builder.Property(x => x.BatchNumber)
            .HasMaxLength(64);

        builder.Property(x => x.ExpiryDate);

        builder.HasIndex(x => x.ItemId);
    }
}
