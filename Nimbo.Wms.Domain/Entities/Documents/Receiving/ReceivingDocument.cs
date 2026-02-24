using JetBrains.Annotations;
using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Entities.Documents.Common;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Entities.Documents.Receiving;

[PublicAPI]
public class ReceivingDocument : DocumentBase<ReceivingDocumentId, ReceivingStatus, ReceivingDocumentLine>
{
    private ReceivingDocument()
    {
        // Required by EF Core
    }
    
    public ReceivingDocument(ReceivingDocumentId id, WarehouseId warehouseId, string code, string title, DateTime createdAt)
        : base(id, code, title, createdAt)
    {
        WarehouseId = warehouseId;
    }
    
    public WarehouseId WarehouseId { get; } 

    public Guid AddLine(ItemId itemId, Quantity receivedQuantity, LocationId toLocationId, Quantity? expectedQuantity, string? notes)
    {
        EnsurePositive(receivedQuantity);
        EnsureNullableNonNegative(expectedQuantity);
        var line = new ReceivingDocumentLine(Id, itemId, receivedQuantity, toLocationId, expectedQuantity, notes);
        AddLine(line, DateTime.UtcNow);
        return line.Id;
    }

    public void RemoveLine(Guid lineId) => RemoveLine(lineId);

    public void ChangeLineReceivedQuantity(Guid lineId, Quantity receivedQty)
    {
        EnsureCanBeEdited();
        EnsurePositive(receivedQty);
        ChangeLineQuantity(lineId, receivedQty);
        Touch();
    }

    public void ChangeLineExpectedQuantity(Guid lineId, Quantity? expectedQty)
    {
        EnsureCanBeEdited();
        EnsureNullableNonNegative(expectedQty);

        var line = GetLine(lineId);
        line.ChangeExpectedQuantity(expectedQty);
        Touch();
    }

    public void ChangeLineToLocation(Guid lineId, LocationId toLocationId)
    {
        EnsureCanBeEdited();
        var line = GetLine(lineId);
        line.ChangeToLocationId(toLocationId);
        Touch();
    }

    public void Start() => TransitionTo(ReceivingStatus.InProgress);
    
    public void Cancel() => TransitionTo(ReceivingStatus.Cancelled);

    public void Post()
    {
        EnsureLinesAreValid();
        TransitionTo(ReceivingStatus.Posted);
        MarkPosted();
    }

    protected override void ValidateTransition(ReceivingStatus currentStatus, ReceivingStatus newStatus)
    {
        if (currentStatus == newStatus)
            return;

        if (currentStatus == ReceivingStatus.Draft && newStatus is ReceivingStatus.InProgress or ReceivingStatus.Cancelled)
            return;

        if (currentStatus == ReceivingStatus.InProgress && newStatus is ReceivingStatus.Posted or ReceivingStatus.Cancelled)
            return;

        if (currentStatus is ReceivingStatus.Posted or ReceivingStatus.Cancelled)
            throw new DomainException($"Invalid transition: {currentStatus} -> {newStatus}.");
    }

    protected override void EnsureLinesAreValid()
    {
        base.EnsureLinesAreValid();
        
        foreach (var line in Lines)
        {
            if (line.ReceivedQuantity.Value <= 0m)
                throw new DomainException($"Line '{line.Id}' has non-positive received quantity.");

            if (line.ExpectedQuantity is not null && line.ExpectedQuantity.Value < 0m)
                throw new DomainException($"Line '{line.Id}' has negative expected quantity.");

            // optional strictness:
            // if (line.ExpectedQuantity is not null && line.ReceivedQuantity.Value > line.ExpectedQuantity.Value)
            //     throw new DomainException($"Line '{line.Id}' received quantity exceeds expected quantity.");
        }

        var duplicate = Lines.GroupBy(x => x.ItemId)
            .FirstOrDefault(g => g.Count() > 1);

        if (duplicate is not null)
            throw new DomainException($"Document cannot have duplicate lines for the same item.");
    }

    private static void EnsurePositive(Quantity q)
    {
        if (q.Value <= 0m)
            throw new DomainException("Quantity must be greater than zero.");
    }

    private static void EnsureNullableNonNegative(Quantity? q)
    {
        if (q is not null && q.Value < 0m)
            throw new DomainException("Expected quantity cannot be negative.");
    }
}
