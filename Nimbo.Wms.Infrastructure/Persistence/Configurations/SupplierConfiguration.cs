using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Infrastructure.Persistence.Converters;

namespace Nimbo.Wms.Infrastructure.Persistence.Configurations;

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("suppliers");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasEntityIdConversion();
        
        builder.Property(x => x.Code)
            .HasMaxLength(Supplier.CodeMaxLength)
            .IsRequired();
        
        builder.Property(x => x.Name)
            .HasMaxLength(Supplier.NameMaxLength)
            .IsRequired();
        
        builder.Property(x => x.TaxId)
            .HasMaxLength(Supplier.TaxIdMaxLength);

        builder.Property(x => x.Address)
            .HasMaxLength(Supplier.AddressMaxLength);

        builder.Property(x => x.ContactName)
            .HasMaxLength(Supplier.ContactNameMaxLength);

        builder.Property(x => x.Phone)
            .HasMaxLength(Supplier.PhoneMaxLength);

        builder.Property(x => x.Email)
            .HasMaxLength(Supplier.EmailMaxLength);

        builder.Property(x => x.IsActive)
            .IsRequired();
        
        // Locations collection (backing field)
        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(l => l.SupplierId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Items)
            .HasField("_items")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(x => x.Code).IsUnique();
        builder.HasIndex(x => x.IsActive);
    }
}
