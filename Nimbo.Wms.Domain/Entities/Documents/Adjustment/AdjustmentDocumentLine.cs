using JetBrains.Annotations;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Entities.Documents.Adjustment;

[PublicAPI]
public sealed class AdjustmentDocumentLine : DocumentLineBase<AdjustmentDocumentId>
{
    private AdjustmentDocumentLine()
    {
        // Required by EF Core
    }

    public AdjustmentDocumentLine(
        AdjustmentDocumentId documentId,
        ItemId itemId,
        LocationId locationId,
        QuantityDelta delta,
        StockLotId? stockLotId = null,
        string? notes = null)
        : base(documentId, itemId, delta.GetAbsQuantity(), notes)
    {
        LocationId = locationId;
        Delta = delta;
        StockLotId = stockLotId;
    }

    public LocationId LocationId { get; private set; }

    public override Quantity Quantity => Delta.GetAbsQuantity();

    public QuantityDelta Delta { get; private set; }

    /// <summary>
    /// Which stock lot is being adjusted. Optional when only one lot exists at the item/location;
    /// required to attribute a positive adjustment with no prior InventoryItem row.
    /// </summary>
    public StockLotId? StockLotId { get; private set; }

    public void ChangeDelta(QuantityDelta delta) => Delta = delta;

    public void ChangeLocation(LocationId locationId) => LocationId = locationId;

    public void ChangeStockLotId(StockLotId? stockLotId) => StockLotId = stockLotId;
}
