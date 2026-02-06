using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Domain.Entities.MasterData;

public class SupplierItem : BaseEntity<SupplierItemId>
{
    // ReSharper disable once UnusedMember.Global
    public SupplierItem()
    {
        // Required by EF Core
    }

    /// <exception cref="ArgumentException">Thrown when the provided strings of supplierSku or supplierBarcode are empty or whitespace or when defaultPurchasePrice, unitsPerPurchaseUom, leadTimeDays or minOrderQty are negative</exception>
    internal SupplierItem(
        SupplierItemId id,
        SupplierId supplierId,
        ItemId itemId,
        string? supplierSku = null,
        string? supplierBarcode = null,
        decimal? defaultPurchasePrice = null,
        string? purchaseUomCode = null,
        decimal? unitsPerPurchaseUom = null,
        int? leadTimeDays = null,
        int? minOrderQty = null,
        bool isPreferred = false)
    {
        Id = id;

        SupplierId = supplierId;
        ItemId = itemId;

        SupplierSku = TrimOrNull(supplierSku);
        SupplierBarcode = TrimOrNull(supplierBarcode);

        DefaultPurchasePrice = RequireNonNegativeOrNull(defaultPurchasePrice, nameof(defaultPurchasePrice));

        PurchaseUomCode = TrimOrNull(purchaseUomCode);
        UnitsPerPurchaseUom = RequirePositiveOrNull(unitsPerPurchaseUom, nameof(unitsPerPurchaseUom));

        LeadTimeDays = RequireNonNegativeIntOrNull(leadTimeDays, nameof(leadTimeDays));
        MinOrderQty = RequireNonNegativeIntOrNull(minOrderQty, nameof(minOrderQty));

        IsPreferred = isPreferred;
    }
    
    public SupplierId SupplierId { get; private set; }

    public ItemId ItemId { get; private set; }

    public string? SupplierSku { get; private set; }

    public string? SupplierBarcode { get; private set; }

    public decimal? DefaultPurchasePrice { get; private set; }

    /// <summary>
    /// Purchase unit code used by supplier (e.g. case, box, pallet).
    /// Note: not necessarily the same as Item.BaseUom.
    /// </summary>
    public string? PurchaseUomCode { get; private set; }

    /// <summary>
    /// Conversion factor from purchase UoM to Item base UoM.
    /// Must be > 0 if specified.
    /// </summary>
    public decimal? UnitsPerPurchaseUom { get; private set; }

    public int? LeadTimeDays { get; private set; }

    public int? MinOrderQty { get; private set; }

    public bool IsPreferred { get; private set; }
    
    public void SetSupplierSku(string? sku) => SupplierSku = TrimOrNull(sku);

    public void SetSupplierBarcode(string? barcode) => SupplierBarcode = TrimOrNull(barcode);

    public void SetDefaultPurchasePrice(decimal? price)
        => DefaultPurchasePrice = RequireNonNegativeOrNull(price, nameof(price));

    public void SetPurchaseUom(string? purchaseUomCode, decimal? unitsPerPurchaseUom)
    {
        PurchaseUomCode = TrimOrNull(purchaseUomCode);
        UnitsPerPurchaseUom = RequirePositiveOrNull(unitsPerPurchaseUom, nameof(unitsPerPurchaseUom));
    }

    public void SetLeadTimeDays(int? days)
        => LeadTimeDays = RequireNonNegativeIntOrNull(days, nameof(days));

    public void SetMinOrderQty(int? qty)
        => MinOrderQty = RequireNonNegativeIntOrNull(qty, nameof(qty));

    public void MarkPreferred() => IsPreferred = true;

    public void UnmarkPreferred() => IsPreferred = false;

    private static string? TrimOrNull(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static decimal? RequirePositiveOrNull(decimal? value, string paramName)
    {
        if (value is not null && value.Value <= 0m)
            throw new ArgumentOutOfRangeException(paramName, "Value must be greater than zero.");
        return value;
    }

    private static decimal? RequireNonNegativeOrNull(decimal? value, string paramName)
    {
        if (value is not null && value.Value < 0m)
            throw new ArgumentOutOfRangeException(paramName, "Value cannot be negative.");
        return value;
    }

    private static int? RequireNonNegativeIntOrNull(int? value, string paramName)
    {
        if (value is not null && value.Value < 0)
            throw new ArgumentOutOfRangeException(paramName, "Value cannot be negative.");
        return value;
    }
}
