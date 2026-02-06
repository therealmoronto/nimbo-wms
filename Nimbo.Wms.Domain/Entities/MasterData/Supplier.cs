using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Domain.Entities.MasterData;

public class Supplier : BaseEntity<SupplierId>
{
    private readonly List<SupplierItem> _items = new();

    // ReSharper disable once UnusedMember.Global
    public Supplier()
    {
        // Required by EF Core
    }

    /// <exception cref="ArgumentException">Thrown when the provided strings of code or name are empty or whitespace</exception>
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

    public string Code { get; private set; }
    
    public string Name { get; private set; }

    public string? TaxId { get; private set; }

    public string? Address { get; private set; }

    public string? ContactName { get; private set; }

    public string? Phone { get; private set; }

    public string? Email { get; private set; }

    public bool IsActive { get; private set; }
    
    public IReadOnlyCollection<SupplierItem> Items => _items.AsReadOnly();
    
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

    public SupplierItem AddItem(
        SupplierItemId supplierItemId,
        ItemId itemId,
        string? supplierSku,
        string? supplierBarcode,
        decimal? defaultPurchasePrice,
        string? purchaseUomCode,
        int? unitsPerPurchaseUom,
        int? leadTimeDays,
        int? minOrderQty,
        bool isPreferred)
    {
        if (_items.Any(x => x.ItemId.Equals(itemId)))
            throw new DomainException("Item is already linked to this supplier.");

        var supplierItem = new SupplierItem(
            supplierItemId,
            Id,
            itemId,
            supplierSku,
            supplierBarcode,
            defaultPurchasePrice,
            purchaseUomCode,
            unitsPerPurchaseUom,
            leadTimeDays,
            minOrderQty,
            isPreferred);

        _items.Add(supplierItem);
        return supplierItem;
    }

    public bool RemoveItem(ItemId itemId)
    {
        var link = _items.SingleOrDefault(x => x.ItemId.Equals(itemId));
        if (link is null)
            return false;

        return _items.Remove(link);
    }
    
    private static string RequireNonEmpty(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be empty.", paramName);

        return value.Trim();
    }

    private static string? TrimOrNull(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
