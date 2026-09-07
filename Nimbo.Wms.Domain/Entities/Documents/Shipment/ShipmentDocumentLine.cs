using JetBrains.Annotations;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Entities.Documents.Shipment;

[PublicAPI]
public sealed class ShipmentDocumentLine : DocumentLineBase<ShipmentDocumentId>
{
    private ShipmentDocumentLine()
    {
        // Required by EF Core
    }

    public ShipmentDocumentLine(ShipmentDocumentId documentId, ItemId itemId, StockLotId? stockLotId, Quantity quantity, string? notes = null)
        : base(documentId, itemId, quantity, notes)
    {
        StockLotId = stockLotId;
    }

    public Quantity RequestedQuantity => Quantity;

    /// <summary>
    /// Which stock lot is requested. Optional — the requested line doesn't need to pin a lot; that
    /// happens at pick time (see ShipmentPickLine.StockLotId, which is mandatory).
    /// </summary>
    public StockLotId? StockLotId { get; private set; }
}
