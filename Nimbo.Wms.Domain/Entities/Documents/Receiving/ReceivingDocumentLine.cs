using JetBrains.Annotations;
using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Entities.Documents.Common;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Entities.Documents.Receiving;

[PublicAPI]
public sealed class ReceivingDocumentLine : DocumentLineBase<ReceivingDocumentId>
{
    public ReceivingDocumentLine()
    {
        // Required by EF Core
    }

    public ReceivingDocumentLine(
        ReceivingDocumentId documentId,
        ItemId itemId,
        Quantity receivedQuantity,
        LocationId toLocationId,
        Quantity? expectedQuantity = null,
        string? notes = null)
        : base(documentId, itemId, receivedQuantity, notes)
    {
        ToLocationId = toLocationId;
        ExpectedQuantity = expectedQuantity;
    }

    public LocationId ToLocationId { get; private set; }

    public Quantity ReceivedQuantity => Quantity;

    public Quantity? ExpectedQuantity { get; private set; }

    public void ChangeExpectedQuantity(Quantity? newExpectedQuantity) => ExpectedQuantity = newExpectedQuantity;

    public void ChangeToLocationId(LocationId locationId) => ToLocationId = locationId;
}
