using JetBrains.Annotations;
using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Entities.Documents.Common;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Entities.Documents.Relocation;

[PublicAPI]
public class RelocationDocument : DocumentBase<RelocationDocumentId, RelocationStatus, RelocationDocumentLine>
{
    public RelocationDocument()
    {
        // Required by EF Core
    }
    
    public RelocationDocument(RelocationDocumentId id, WarehouseId warehouseId, string code, string title, DateTime createdAt)
        : base(id, code, title, createdAt)
    {
        WarehouseId = warehouseId;
    }

    public WarehouseId WarehouseId { get; private set; }

    public Guid AddLine(ItemId itemId, Quantity quantity, LocationId from, LocationId to, string? notes = null)
    {
        EnsureCanBeEdited();
        EnsurePositive(quantity);

        if (from == to)
            throw new DomainException("FromLocationId and ToLocationId cannot be the same.");

        if (Lines.Any(x => x.ItemId == itemId && x.From == from && x.To == to))
            throw new DomainException("Duplicate relocation line (same item and same from/to).");

        var line = new RelocationDocumentLine(Id, itemId, from, to, quantity, notes);
        AddLine(line, DateTime.UtcNow);

        return line.Id;
    }
    
    public void ChangeLineQuantity(Guid lineId, Quantity quantity)
    {
        EnsureCanBeEdited();
        EnsurePositive(quantity);

        var line = GetLine(lineId);
        line.ChangeQuantity(quantity);
        Touch();
    }
    public void ChangeLineFrom(Guid lineId, LocationId from)
    {
        EnsureCanBeEdited();

        var line = GetLine(lineId);
        if (from == line.To)
            throw new DomainException("FromLocationId and ToLocationId cannot be the same.");

        line.ChangeFrom(from);
        EnsureNoDuplicates();
        Touch();
    }

    public void ChangeLineTo(Guid lineId, LocationId to)
    {
        EnsureCanBeEdited();

        var line = GetLine(lineId);
        if (to == line.From)
            throw new DomainException("FromLocationId and ToLocationId cannot be the same.");

        line.ChangeTo(to);
        EnsureNoDuplicates();
        Touch();
    }

    public void Start() => TransitionTo(RelocationStatus.InProgress);

    public void Cancel() => TransitionTo(RelocationStatus.Cancelled);

    public void Post()
    {
        EnsureLinesAreValid();
        TransitionTo(RelocationStatus.Posted);
        MarkPosted();
    }

    protected override void EnsureLinesAreValid()
    {
        if (Lines.Count == 0)
            throw new DomainException("Relocation document must contain at least one line.");

        foreach (var line in Lines)
        {
            if (line.Quantity.Value <= 0m)
                throw new DomainException($"Line '{line.Id}' has non-positive quantity.");

            if (line.From == line.To)
                throw new DomainException($"Line '{line.Id}' has same from/to location.");
        }

        EnsureNoDuplicates();
    }

    protected override void ValidateTransition(RelocationStatus currentStatus, RelocationStatus newStatus)
    {
        if (currentStatus == newStatus)
            return;

        if (currentStatus == RelocationStatus.Draft && newStatus is RelocationStatus.InProgress or RelocationStatus.Cancelled)
            return;

        if (currentStatus == RelocationStatus.InProgress && newStatus is RelocationStatus.Posted or RelocationStatus.Cancelled)
            return;

        if (currentStatus is RelocationStatus.Posted or RelocationStatus.Cancelled)
            throw new DomainException($"Invalid transition: {currentStatus} -> {newStatus}.");
    }

    private void EnsureNoDuplicates()
    {
        var dup = Lines
            .GroupBy(x => new { x.ItemId, x.From, x.To })
            .FirstOrDefault(g => g.Count() > 1);

        if (dup is not null)
            throw new DomainException("Relocation document contains duplicate lines (same item and same from/to).");
    }

    private static void EnsurePositive(Quantity quantity)
    {
        if (quantity.Value <= 0m)
            throw new DomainException("Quantity must be greater than zero.");
    }
}
