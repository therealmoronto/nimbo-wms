using JetBrains.Annotations;
using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Entities.Documents.Common;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Entities.Documents.CycleCount;

[PublicAPI]
public sealed class CycleCountDocumentLine : DocumentLineBase<CycleCountDocumentId>
{
    private CycleCountDocumentLine()
    {
        // Required by EF Core
    }

    public CycleCountDocumentLine(
        CycleCountDocumentId documentId,
        LocationId locationId,
        ItemId itemId,
        Quantity quantity,
        string? notes = null)
        : base(documentId, itemId, quantity, notes)
    {
        LocationId = locationId;
    }

    public LocationId LocationId { get; private set; }

    public Quantity ExpectedQuantity => Quantity;

    public Quantity? ActualQuantity { get; private set; }

    public void ChangeActualQuantity(Quantity actualQuantity)
    {
        if (actualQuantity.Value < 0m)
            throw new DomainException("Actual quantity cannot be negative.");

        ActualQuantity = actualQuantity;
    }

    public decimal GetDelta()
    {
        if (ActualQuantity is null)
            throw new DomainException("Actual quantity is not set.");

        return ActualQuantity.Value - ExpectedQuantity.Value;
    }
}
