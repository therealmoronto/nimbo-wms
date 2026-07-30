using JetBrains.Annotations;
using Nimbo.Wms.Domain.Entities.Documents.Common;
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

    public ShipmentDocumentLine(ShipmentDocumentId documentId, ItemId itemId, BatchId? batchId, Quantity quantity, string? notes = null)
        : base(documentId, itemId, quantity, notes)
    {
        BatchId = batchId;
    }

    public BatchId? BatchId { get; private set; }

    public Quantity RequestedQuantity => Quantity;
}
