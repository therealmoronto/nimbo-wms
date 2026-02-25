using JetBrains.Annotations;
using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Entities.Documents.Common;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Entities.Documents.Shipment;

[PublicAPI]
public sealed class ShipmentDocument : DocumentBase<ShipmentDocumentId, ShipmentStatus, ShipmentDocumentLine>
{
    public ShipmentDocument()
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

    public void ChangeCustomer(CustomerId? customerId)
    {
        EnsureCanBeEdited();
        if (CustomerId == customerId)
            return;

        CustomerId = customerId;
        Touch();
    }
    
    public void AddLine(ItemId itemId, Quantity requestedQty, string? notes = null)
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
    
    public void ChangeLineShippedQuantity(Guid lineId, Quantity shippedQty)
    {
        EnsureCanBeEdited();
        var line = GetLine(lineId);
        line.ChangeShippedQuantity(shippedQty);
        Touch();
    }
    
    public void Start() => TransitionTo(ShipmentStatus.InProgress);

    public void MarkShipped()
    {
        EnsureLinesAreValidForShipping();
        TransitionTo(ShipmentStatus.Shipped);
    }

    public void Post()
    {
        EnsureLinesAreValidForPosting();
        TransitionTo(ShipmentStatus.Posted);
        MarkPosted();
    }

    protected override void EnsureLinesAreValid()
    {
        if (Lines.Count == 0)
            throw new DomainException("Shipment must contain at least one line.");
    }

    private void EnsureLinesAreValidForShipping()
    {
        EnsureLinesAreValid();

        foreach (var line in Lines)
        {
            if (line.ShippedQuantity is null)
                throw new DomainException("All lines must have shipped quantity set.");
        }
    }

    private void EnsureLinesAreValidForPosting()
    {
        foreach (var line in Lines)
        {
            if (line.ShippedQuantity is null || line.ShippedQuantity.Value <= 0m)
                throw new DomainException("Invalid shipped quantity.");
        }
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
}
