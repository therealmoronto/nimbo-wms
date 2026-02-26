using JetBrains.Annotations;
using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Entities.Documents.Common;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Entities.Documents.CycleCount;

[PublicAPI]
public sealed class CycleCountDocument : DocumentBase<CycleCountDocumentId, CycleCountStatus, CycleCountDocumentLine>
{
    private CycleCountDocument()
    {
        // Required by EF Core
    }
    
    public CycleCountDocument(
        CycleCountDocumentId id,
        WarehouseId warehouseId,
        string code,
        string title,
        DateTime createdAt)
        : base(id, code, title, createdAt)
    {
        WarehouseId = warehouseId;
    }

    public WarehouseId WarehouseId { get; }
    
    public Guid AddLine(ItemId itemId, LocationId locationId, Quantity expectedQty)
    {
        EnsureCanBeEdited();

        if (Lines.Any(x => x.ItemId == itemId && x.LocationId == locationId))
            throw new DomainException("Duplicate cycle count line.");

        var line = new CycleCountDocumentLine(Id, locationId, itemId, expectedQty);
        AddLine(line);

        Touch();

        return line.Id;
    }

    public void ChangeLineActualQuantity(Guid lineId, Quantity actualQty)
    {
        var line = GetLine(lineId);
        line.ChangeActualQuantity(actualQty);
        Touch();
    }

    public void StartCounting() => TransitionTo(CycleCountStatus.Counting);

    public void Complete()
    {
        EnsureAllLinesCounted();
        TransitionTo(CycleCountStatus.Completed);
    }

    public void Post()
    {
        EnsureAllLinesCounted();

        TransitionTo(CycleCountStatus.Posted);
        MarkPosted();
    }

    public void Cancel() => TransitionTo(CycleCountStatus.Cancelled);

    protected override void ValidateTransition(CycleCountStatus current, CycleCountStatus next)
    {
        if (current == next)
            return;

        if (current is CycleCountStatus.Draft && next is CycleCountStatus.Counting or CycleCountStatus.Cancelled)
            return;

        if (current is CycleCountStatus.Counting && next is CycleCountStatus.Completed or CycleCountStatus.Cancelled)
            return;

        if (current is CycleCountStatus.Completed && next is CycleCountStatus.Posted)
            return;

        if (current is CycleCountStatus.Posted or CycleCountStatus.Cancelled)
            throw new DomainException($"Invalid transition: {current} -> {next}.");
    }

    private void EnsureAllLinesCounted()
    {
        if (Lines.Count == 0)
            throw new DomainException("Cycle count must contain lines.");

        if (Lines.Any(x => x.ActualQuantity is null))
            throw new DomainException("All lines must have actual quantity set.");
    }
}
