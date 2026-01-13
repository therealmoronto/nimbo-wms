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
            .HasMaxLength(32)
            .IsRequired();
        
        builder.Property(x => x.Name)
            .HasMaxLength(128)
            .IsRequired();
        
        builder.Property(x => x.TaxId)
            .HasMaxLength(64);

        builder.Property(x => x.Address)
            .HasMaxLength(512);

        builder.Property(x => x.ContactName)
            .HasMaxLength(512);

        builder.Property(x => x.Phone)
            .HasMaxLength(64);

        builder.Property(x => x.Email)
            .HasMaxLength(16);

        builder.Property(x => x.IsActive)
            .IsRequired();
        
        builder.HasIndex(x => x.Code).IsUnique();
        builder.HasIndex(x => x.IsActive);
    }
}
