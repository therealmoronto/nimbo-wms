using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Domain.Entities.Stock;

/// <summary>
/// The system's own receiving lineage: which ReceivingDocument, and when. Always created, once per
/// receiving line, regardless of Item.IsBatchManaged — this is what makes FIFO/FEFO rotation possible
/// for every item, not just batch-managed ones. Optionally linked to a <see cref="VendorLot"/> when the
/// item is batch-managed. Pure lineage — no location/warehouse; <see cref="InventoryItem"/> owns that.
/// </summary>
public class StockLot : BaseEntity<StockLotId>
{
    // ReSharper disable once UnusedMember.Global
    public StockLot()
    {
        // Required by EF Core
    }

    /// <exception cref="ArgumentOutOfRangeException">Thrown when unitCost is negative</exception>
    public StockLot(
        StockLotId id,
        ItemId itemId,
        ReceivingDocumentId receivingDocumentId,
        DateTime receivedAt,
        VendorLotId? vendorLotId = null,
        decimal? unitCost = null)
    {
        Id = id;

        ItemId = itemId;
        ReceivingDocumentId = receivingDocumentId;
        ReceivedAt = receivedAt;
        VendorLotId = vendorLotId;
        UnitCost = RequireNonNegativeOrNull(unitCost, nameof(unitCost));
    }

    public ItemId ItemId { get; }

    public ReceivingDocumentId ReceivingDocumentId { get; }

    public DateTime ReceivedAt { get; }

    public VendorLotId? VendorLotId { get; private set; }

    public decimal? UnitCost { get; private set; }

    public void SetUnitCost(decimal? unitCost) => UnitCost = RequireNonNegativeOrNull(unitCost, nameof(unitCost));

    private static decimal? RequireNonNegativeOrNull(decimal? value, string paramName)
    {
        if (value is not null && value.Value < 0m)
            throw new ArgumentOutOfRangeException(paramName, "Value cannot be negative.");
        return value;
    }
}
