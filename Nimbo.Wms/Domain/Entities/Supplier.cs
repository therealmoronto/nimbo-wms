using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Domain.Entities;

public class Supplier : IEntity<SupplierId>
{
    public Supplier(
        SupplierId id,
        string code,
        string name,
        string? taxId = null,
        string? address = null,
        string? contactName = null,
        string? phone = null,
        string? email = null,
        bool isActive = true)
    {
        Id = id;

        Code = RequireNonEmpty(code, nameof(code));
        Name = RequireNonEmpty(name, nameof(name));

        TaxId = TrimOrNull(taxId);
        Address = TrimOrNull(address);

        ContactName = TrimOrNull(contactName);
        Phone = TrimOrNull(phone);
        Email = TrimOrNull(email);

        IsActive = isActive;
    }
    
    public SupplierId Id { get; }
    
    public string Code { get; private set; }
    
    public string Name { get; private set; }

    public string? TaxId { get; private set; }

    public string? Address { get; private set; }

    public string? ContactName { get; private set; }

    public string? Phone { get; private set; }

    public string? Email { get; private set; }

    public bool IsActive { get; private set; }
    
    public void Rename(string name) => Name = RequireNonEmpty(name, nameof(name));

    public void ChangeCode(string code) => Code = RequireNonEmpty(code, nameof(code));

    public void SetTaxId(string? taxId) => TaxId = TrimOrNull(taxId);

    public void SetAddress(string? address) => Address = TrimOrNull(address);

    public void SetContact(string? contactName, string? phone, string? email)
    {
        ContactName = TrimOrNull(contactName);
        Phone = TrimOrNull(phone);
        Email = TrimOrNull(email);
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;

    private static string RequireNonEmpty(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be empty.", paramName);
        return value.Trim();
    }

    private static string? TrimOrNull(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
