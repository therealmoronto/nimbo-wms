using JetBrains.Annotations;
using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Entities.Documents.Common;
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
        LocationId fromLocation,
        Quantity quantity,
        string? notes = null) : base(documentId, itemId, quantity, notes)
    {
        if (quantity.Value <= 0m)
            throw new DomainException("Pick quantity must be greater than zero.");

        FromLocation = fromLocation;
    }

    public LocationId FromLocation { get; private set; }

    public void ChangeFromLocation(LocationId fromLocationId) => FromLocation = fromLocationId;
}
