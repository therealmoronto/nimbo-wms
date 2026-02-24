using JetBrains.Annotations;
using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Entities.Documents.Common;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Entities.Documents.Adjustment;

[PublicAPI]
public sealed class AdjustmentDocument : DocumentBase<AdjustmentDocumentId, AdjustmentStatus, AdjustmentDocumentLine>
{
    private AdjustmentDocument()
    {
        // Required by EF Core
    }

    public AdjustmentDocument(AdjustmentDocumentId id, WarehouseId warehouseId, string code, string title, DateTime createdAt)
        : base(id, code, title, createdAt)
    {
        WarehouseId = warehouseId;
    }

    public WarehouseId WarehouseId { get; }

    public string ReasonCode { get; private set; } = "UNSPECIFIED";

    public string? ReasonText { get; private set; }

    public void ChangeReason(string reasonCode, string? reasonText)
    {
        EnsureCanBeEdited();
        
        if (string.IsNullOrWhiteSpace(reasonCode))
            throw new DomainException("ReasonCode cannot be empty.");

        ReasonCode = reasonCode.Trim();
        ReasonText = !string.IsNullOrWhiteSpace(reasonText) ? reasonText.Trim() : null;
        Touch();
    }

    public Guid AddLine(ItemId itemId, LocationId locationId, QuantityDelta delta, string? notes = null)
    {
        EnsureCanBeEdited();

        if (delta.Value == 0m)
            throw new DomainException("Delta cannot be zero.");

        // duplicates by (Item, Location) is a sane MVP rule
        if (Lines.Any(x => x.ItemId == itemId && x.LocationId == locationId))
            throw new DomainException("Duplicate adjustment line (same item and location).");

        var line = new AdjustmentDocumentLine(Id, itemId, locationId, delta, notes);
        AddLine(line);
        return line.Id;
    }

    public void ChangeLineDelta(Guid lineId, QuantityDelta delta)
    {
        EnsureCanBeEdited();
        if (delta.Value == 0m) throw new DomainException("Delta cannot be zero.");

        var line = GetLine(lineId);
        line.ChangeDelta(delta);
        Touch();
    }
    
    public void ChangeLineLocation(Guid lineId, LocationId locationId)
    {
        EnsureCanBeEdited();

        var line = GetLine(lineId);
        line.ChangeLocation(locationId);
        EnsureNoDuplicates();
        Touch();
    }

    public void Approve() => TransitionTo(AdjustmentStatus.Approved);

    public void Cancel() => TransitionTo(AdjustmentStatus.Cancelled);

    public void Post()
    {
        EnsureLinesAreValid();
        TransitionTo(AdjustmentStatus.Posted);
        MarkPosted();
    }

    protected override void ValidateTransition(AdjustmentStatus currentStatus, AdjustmentStatus newStatus)
    {
        if (currentStatus == newStatus)
            return;

        if (currentStatus is AdjustmentStatus.Draft && newStatus is AdjustmentStatus.Approved or AdjustmentStatus.Cancelled)
            return;

        if (currentStatus is AdjustmentStatus.Approved && newStatus is AdjustmentStatus.Posted or AdjustmentStatus.Cancelled)
            return;

        if (currentStatus is AdjustmentStatus.Posted or AdjustmentStatus.Cancelled)
            throw new DomainException($"Invalid transition: {currentStatus} -> {newStatus}.");
    }

    protected override void EnsureLinesAreValid()
    {
        if (Lines.Count == 0)
            throw new DomainException("Adjustment document must contain at least one line.");

        foreach (var line in Lines)
        {
            if (line.Delta.Value == 0m)
                throw new DomainException($"Line '{line.Id}' has zero delta.");
        }

        EnsureNoDuplicates();
    }

    private void EnsureNoDuplicates()
    {
        var dup = Lines.GroupBy(x => new { x.ItemId, x.LocationId }).FirstOrDefault(g => g.Count() > 1);
        if (dup is not null)
            throw new DomainException("Adjustment document contains duplicate lines (same item and location).");
    }
}
