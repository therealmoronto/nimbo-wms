using JetBrains.Annotations;
using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Entities.Documents.Shipment;

[PublicAPI]
public sealed class ShipmentPickLine : DocumentLineBase<ShipmentDocumentId>
{
    private ShipmentPickLine()
    {
        // Required by EF Core
    }

    public ShipmentPickLine(
        ShipmentDocumentId documentId,
        ItemId itemId,
        StockLotId stockLotId,
        LocationId fromLocation,
        Quantity quantity,
        string? notes = null) : base(documentId, itemId, quantity, notes)
    {
        if (quantity.Value <= 0m)
            throw new DomainException("Pick quantity must be greater than zero.");

        StockLotId = stockLotId;
        FromLocation = fromLocation;
    }

    public LocationId FromLocation { get; private set; }

    /// <summary>
    /// Which stock lot to decrement. Mandatory — once more than one lot can coexist at an item/location
    /// (the whole point of FIFO/FEFO), an unspecified pick lot is ambiguous. The caller is expected to
    /// have already chosen one via the available-stock-lots query.
    /// </summary>
    public StockLotId StockLotId { get; private set; }

    public void ChangeFromLocation(LocationId fromLocationId) => FromLocation = fromLocationId;
}
