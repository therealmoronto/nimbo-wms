using JetBrains.Annotations;
using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Entities.Documents.Common;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Entities.Documents.Shipment;

[PublicAPI]
public sealed class ShipmentDocumentLine : DocumentLineBase<ShipmentDocumentId>
{
    public ShipmentDocumentLine()
    {
        // Required by EF Core
    }

    public ShipmentDocumentLine(ShipmentDocumentId documentId, ItemId itemId, Quantity quantity, string? notes = null)
        : base(documentId, itemId, quantity, notes)
    {
        
    }

    public Quantity RequestedQuantity => Quantity;

    public Quantity? ShippedQuantity { get; private set; }

    public void ChangeShippedQuantity(Quantity quantity)
    {
        if (quantity.Value < 0m)
            throw new DomainException("Shipped quantity cannot be negative.");

        if (quantity.Value > RequestedQuantity.Value)
            throw new DomainException("Shipped quantity cannot exceed requested quantity.");

        ShippedQuantity = quantity;
    }
}
