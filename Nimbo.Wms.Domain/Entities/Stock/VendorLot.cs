using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Domain.Entities.Stock;

/// <summary>
/// The supplier's declared batch identity: item + batch number + supplier + expiry.
/// Optional — only created when Item.IsBatchManaged == true. Contrast with <see cref="StockLot"/>,
/// which is always created per receiving line regardless of IsBatchManaged.
/// </summary>
public class VendorLot : BaseEntity<VendorLotId>
{
    public const int BatchNumberMaxLength = 128;

    // ReSharper disable once UnusedMember.Global
    public VendorLot()
    {
        // Required by EF Core
    }

    /// <exception cref="ArgumentException">Thrown when batchNumber is empty or whitespace</exception>
    public VendorLot(
        VendorLotId id,
        ItemId itemId,
        string batchNumber,
        SupplierId? supplierId = null,
        DateTime? expiryDate = null)
    {
        Id = id;

        ItemId = itemId;
        BatchNumber = RequireNonEmpty(batchNumber, nameof(batchNumber));
        SupplierId = supplierId;
        ExpiryDate = expiryDate;
    }

    public ItemId ItemId { get; }

    public string BatchNumber { get; private set; } = null!;

    public SupplierId? SupplierId { get; private set; }

    public DateTime? ExpiryDate { get; private set; }

    public void SetSupplier(SupplierId? supplierId) => SupplierId = supplierId;

    public void SetExpiryDate(DateTime? expiryDate) => ExpiryDate = expiryDate;

    private static string RequireNonEmpty(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be empty.", paramName);

        return value.Trim();
    }
}
