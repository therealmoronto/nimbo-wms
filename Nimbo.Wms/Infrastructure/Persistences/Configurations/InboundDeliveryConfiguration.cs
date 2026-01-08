using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Documents.Inbound;
using Nimbo.Wms.Infrastructure.Persistences.Converters;

namespace Nimbo.Wms.Infrastructure.Persistences.Configurations;

public class InboundDeliveryConfiguration : IEntityTypeConfiguration<InboundDelivery>
{
    public void Configure(EntityTypeBuilder<InboundDelivery> builder)
    {
        builder.ToTable("inbound_deliveries");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasEntityIdConversion();
        
        builder.Property(x => x.SupplierId)
            .HasEntityIdConversion()
            .IsRequired();
        
        builder.Property(x => x.WarehouseId)
            .HasEntityIdConversion()
            .IsRequired();
        
        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.ExternalReference)
            .HasMaxLength(128);

        builder.Property(x => x.StartedAt);

        builder.Property(x => x.StartedAt);
                
        builder.Metadata
            .FindNavigation(nameof(InboundDelivery.Lines))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany<InboundDeliveryLine>("_lines")
            .WithOne()
            .HasForeignKey(l => l.InboundDeliveryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation("_lines");

        builder.HasIndex(x => x.SupplierId);
        builder.HasIndex(x => x.WarehouseId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.CreatedAt);
    }
}
