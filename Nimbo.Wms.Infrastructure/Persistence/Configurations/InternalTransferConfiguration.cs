using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Entities.Movements;
using Nimbo.Wms.Domain.ValueObject;
using Nimbo.Wms.Infrastructure.Persistence.Converters;

namespace Nimbo.Wms.Infrastructure.Persistence.Configurations;

public class InternalTransferConfiguration : IEntityTypeConfiguration<InternalTransfer>
{
    public void Configure(EntityTypeBuilder<InternalTransfer> builder)
    {
        builder.ToTable("internal_transfers");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasEntityIdConversion();
        
        builder.Property(x => x.WarehouseId)
            .HasEntityIdConversion()
            .IsRequired();
        
        builder.Property(x => x.ItemId)
            .HasEntityIdConversion()
            .IsRequired();
        
        builder.Property(x => x.FromLocationId)
            .HasEntityIdConversion()
            .IsRequired();
        
        builder.Property(x => x.ToLocationId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.OwnsOne<Quantity>(
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

        builder.Property(x => x.OccurredAt)
            .IsRequired();

        builder.Property(x => x.Reason)
            .HasMaxLength(256);
    }
}
