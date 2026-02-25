using JetBrains.Annotations;
using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Entities.Documents.Common;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Entities.Documents.Shipment;

[PublicAPI]
public sealed class ShipmentDocument : DocumentBase<ShipmentDocumentId, ShipmentStatus, ShipmentDocumentLine>
{
    private readonly List<ShipmentPickLine> _pickLines = new();
    
    private ShipmentDocument()
    {
        // Required by EF Core
    }

    public ShipmentDocument(ShipmentDocumentId id, WarehouseId warehouseId, string code, string title, DateTime createdAt) 
        : base(id, code, title, createdAt)
    {
        WarehouseId = warehouseId;
    }

    public WarehouseId WarehouseId { get; }

    public CustomerId? CustomerId { get; private set; }

    public IReadOnlyList<ShipmentPickLine> PickLines => _pickLines.AsReadOnly();

    public void ChangeCustomer(CustomerId? customerId)
    {
        EnsureCanBeEdited();
        if (CustomerId == customerId)
            return;

        CustomerId = customerId;
        Touch();
    }
    
    public void AddRequestedLine(ItemId itemId, Quantity requestedQty, string? notes = null)
    {
        EnsureCanBeEdited();

        if (requestedQty.Value <= 0m)
            throw new DomainException("Requested quantity must be greater than zero.");

        if (Lines.Any(x => x.ItemId == itemId))
            throw new DomainException("Duplicate shipment line for same item.");

        var line = new ShipmentDocumentLine(Id, itemId, requestedQty, notes);
        AddLine(line);
        Touch();
    }

    public void AddPickLine(ItemId itemId, LocationId fromLocationId, Quantity qty, string? notes = null)
    {
        EnsureCanBeEdited();

        var planLine = Lines.FirstOrDefault(x => x.ItemId == itemId)
                       ?? throw new DomainException("Cannot pick an item that is not present in shipment lines.");

        if (qty.Value <= 0m)
            throw new DomainException("Pick quantity must be greater than zero.");

        if (Equals(fromLocationId, default(LocationId)))
            throw new DomainException("FromLocationId is required.");

        _pickLines.Add(new ShipmentPickLine(Id, itemId, fromLocationId, qty, notes));

        EnsurePickTotalsDoNotExceedRequested(itemId, planLine.RequestedQuantity);
        Touch();
    }

    public void ChangeLineRequestedQuantity(Guid lineId, Quantity requestedQuantity)
    {
        EnsureCanBeEdited();
        var line = GetLine(lineId);
        line.ChangeQuantity(requestedQuantity);
        Touch();
    }

    public void ChangePickLineQuantity(Guid pickLineId, Quantity qty)
    {
        EnsureCanBeEdited();

        var pl = GetPickLine(pickLineId);
        pl.ChangeQuantity(qty);

        var plan = Lines.First(x => x.ItemId == pl.ItemId);
        EnsurePickTotalsDoNotExceedRequested(pl.ItemId, plan.RequestedQuantity);

        Touch();
    }

    public void ChangePickLineFromLocation(Guid pickLineId, LocationId fromLocationId)
    {
        EnsureCanBeEdited();

        var pl = GetPickLine(pickLineId);
        pl.ChangeFromLocation(fromLocationId);

        Touch();
    }

    public void RemovePickLine(Guid pickLineId)
    {
        EnsureCanBeEdited();

        var index = _pickLines.FindIndex(x => x.Id == pickLineId);
        if (index < 0)
            throw new DomainException($"Pick line '{pickLineId}' not found.");

        _pickLines.RemoveAt(index);
        Touch();
    }

    public void Start() => TransitionTo(ShipmentStatus.InProgress);

    public void MarkShipped()
    {
        EnsureReadyToShip();
        TransitionTo(ShipmentStatus.Shipped);
    }

    public void Post()
    {
        EnsureReadyToPost();
        TransitionTo(ShipmentStatus.Posted);
        MarkPosted();
    }

    protected override void EnsureLinesAreValid()
    {
        if (Lines.Count == 0)
            throw new DomainException("Shipment must contain at least one line.");
    }

    protected override void ValidateTransition(ShipmentStatus current, ShipmentStatus next)
    {
        if (current == next)
            return;

        if (current is ShipmentStatus.Draft && next is ShipmentStatus.InProgress or ShipmentStatus.Cancelled)
            return;

        if (current is ShipmentStatus.InProgress && next is ShipmentStatus.Shipped or ShipmentStatus.Cancelled)
            return;

        if (current is ShipmentStatus.Shipped && next is ShipmentStatus.Posted)
            return;

        if (current is ShipmentStatus.Posted or ShipmentStatus.Cancelled)
            throw new DomainException($"Invalid transition: {current} -> {next}.");
    }

    private ShipmentPickLine GetPickLine(Guid pickLineId)
    {
        return _pickLines.FirstOrDefault(x => x.Id == pickLineId)
               ?? throw new DomainException($"Pick line '{pickLineId}' not found.");
    }

    private void EnsureReadyToShip()
    {
        EnsureLinesAreValid();

        if (_pickLines.Count == 0)
            throw new DomainException("Shipment must contain at least one pick line.");

        // Full shipment policy: picked totals must match requested totals for each item
        foreach (var line in Lines)
        {
            var picked = GetPickedTotalForItem(line.ItemId);
            if (picked != line.RequestedQuantity.Value)
                throw new DomainException("All shipment lines must be fully picked before shipping.");
        }
    }

    private void EnsureReadyToPost()
    {
        if (_pickLines.Any(x => x.Quantity.Value <= 0m))
            throw new DomainException("Invalid pick quantity.");
    }

    private decimal GetPickedTotalForItem(ItemId itemId)
        => _pickLines.Where(x => x.ItemId == itemId).Sum(x => x.Quantity.Value);

    private void EnsurePickTotalsDoNotExceedRequested(ItemId itemId, Quantity requestedQty)
    {
        var picked = GetPickedTotalForItem(itemId);
        if (picked > requestedQty.Value)
            throw new DomainException("Picked quantity exceeds requested quantity for the item.");
    }
}
