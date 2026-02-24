using JetBrains.Annotations;
using Nimbo.Wms.Domain.Entities.Documents.Common;
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
        string? notes = null)
        : base(documentId, itemId, delta.GetAbsQuantity(), notes)
    {
        LocationId = locationId;
        Delta = delta;
    }

    public LocationId LocationId { get; private set; }

    public override Quantity Quantity => Delta.GetAbsQuantity();

    public QuantityDelta Delta { get; private set; }

    public void ChangeDelta(QuantityDelta delta) => Delta = delta;

    public void ChangeLocation(LocationId locationId) => LocationId = locationId;
}
