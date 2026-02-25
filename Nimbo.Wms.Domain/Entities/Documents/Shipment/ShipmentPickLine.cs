using JetBrains.Annotations;
using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Entities.Documents.Shipment;

[PublicAPI]
public sealed class ShipmentPickLine
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
        string? notes = null)
    {
        if (quantity.Value <= 0m)
            throw new DomainException("Pick quantity must be greater than zero.");

        DocumentId = documentId;
        ItemId = itemId;
        FromLocation = fromLocation;
        Quantity = quantity;
        Notes = notes;
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }

    public ShipmentDocumentId DocumentId { get; }

    public ItemId ItemId { get; }

    public LocationId FromLocation { get; private set; }

    public Quantity Quantity { get; private set; }

    public string? Notes { get; private set; }

    public void ChangeQuantity(Quantity quantity)
    {
        if (quantity.Value <= 0m)
            throw new DomainException("Pick quantity must be greater than zero.");

        Quantity = quantity;
    }

    public void ChangeFromLocation(LocationId fromLocationId) => FromLocation = fromLocationId;

    public void SetNotes(string? notes) => Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
}
