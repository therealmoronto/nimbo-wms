using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Infrastructure.Persistence.Converters;

namespace Nimbo.Wms.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasEntityIdConversion();
        
        builder.Property(x => x.Code)
            .HasMaxLength(Customer.CodeMaxLength)
            .IsRequired();
        
        builder.Property(x => x.Name)
            .HasMaxLength(Customer.NameMaxLength)
            .IsRequired();
        
        builder.Property(x => x.TaxId)
            .HasMaxLength(Customer.TaxIdMaxLength);
        
        builder.Property(x => x.Address)
            .HasMaxLength(Customer.AddressMaxLength);
        
        builder.Property(x => x.ContactName)
            .HasMaxLength(Customer.ContactNameMaxLength);

        builder.Property(x => x.Phone)
            .HasMaxLength(Customer.PhoneMaxLength);

        builder.Property(x => x.Email)
            .HasMaxLength(Customer.EmailMaxLength);

        builder.Property(x => x.IsActive)
            .IsRequired();
        
        builder.HasIndex(x => x.Code).IsUnique();
        builder.HasIndex(x => x.IsActive);
    }
}
