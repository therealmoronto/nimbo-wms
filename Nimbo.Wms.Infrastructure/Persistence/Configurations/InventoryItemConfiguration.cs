using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Infrastructure.Persistence.Converters;

namespace Nimbo.Wms.Infrastructure.Persistence.Configurations;

public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.ToTable("inventory_items");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasEntityIdConversion();

        builder.Property(x => x.ItemId)
            .HasEntityIdConversion()
            .IsRequired();
        
        builder.Property(x => x.WarehouseId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.LocationId)
            .HasEntityIdConversion()
            .IsRequired();

        builder.Property(x => x.BatchId)
            .HasEntityIdConversion();

        builder.Property(x => x.SerialNumber)
            .HasMaxLength(128);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.UnitCost)
            .HasColumnType("numeric(18, 4)");   

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
        
        builder.Navigation(x => x.Quantity)
            .IsRequired();

        builder.HasOne<Item>()
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Warehouse>()
            .WithMany()
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Location>()
            .WithMany()
            .HasForeignKey(x => x.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Batch>()
            .WithMany()
            .HasForeignKey(x => x.BatchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new {  x.ItemId, x.WarehouseId, x.LocationId });
        builder.HasIndex(x => x.Status);
    }
}
