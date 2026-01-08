using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Documents.Transfer;
using Nimbo.Wms.Infrastructure.Persistences.Converters;

namespace Nimbo.Wms.Infrastructure.Persistences.Configurations;

public class TransferOrderConfiguration : IEntityTypeConfiguration<TransferOrder>
{
    public void Configure(EntityTypeBuilder<TransferOrder> builder)
    {
        builder.ToTable("transfer_orders");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasEntityIdConversion();
        
        builder.Property(x => x.FromWarehouseId)
            .HasEntityIdConversion()
            .IsRequired();
        
        builder.Property(x => x.ToWarehouseId)
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

        builder.Property(x => x.PickingStartedAt);

        builder.Property(x => x.ShippedAt);

        builder.Property(x => x.ReceivedAt);
        
        builder.Metadata
            .FindNavigation(nameof(TransferOrder.Lines))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany<TransferOrderLine>("_lines")
            .WithOne()
            .HasForeignKey(l => l.TransferOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation("_lines");

        builder.HasIndex(x => x.FromWarehouseId);
        builder.HasIndex(x => x.ToWarehouseId);
        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.Status);
    }
}
