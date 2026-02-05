using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Domain.Documents.Outbound;

public class ShipmentOrder : Document<ShipmentOrderId, ShipmentOrderStatus>
{
    private readonly List<ShipmentOrderLine> _lines = new();

    private ShipmentOrder()
    {
        // Required by EF Core
    }
    
    public ShipmentOrder(
        ShipmentOrderId id,
        string code,
        string name,
        DateTime createdAt,
        WarehouseId warehouseId,
        CustomerId customerId,
        string? externalReference = null)
        : base(id, code, name, ShipmentOrderStatus.Draft, createdAt, externalReference)
    {
        WarehouseId = warehouseId;
        CustomerId = customerId;
    }

    public WarehouseId WarehouseId { get; }

    public CustomerId CustomerId { get; }
    
    public DateTime? ShippedAt { get; private set; }

    public DateTime? CancelledAt { get; private set; }

    public string? CancelReason { get; private set; }

    public IReadOnlyCollection<ShipmentOrderLine> Lines => _lines;

    public ShipmentOrderLine AddLine(ItemId itemId, decimal orderedQty, UnitOfMeasure uomCode)
    {
        EnsureStatus(ShipmentOrderStatus.Draft);

        if (orderedQty <= 0) throw new ArgumentOutOfRangeException(nameof(orderedQty));

        var line = new ShipmentOrderLine(Id, itemId, orderedQty, uomCode);

        _lines.Add(line);
        return line;
    }

    public void Reserve(Guid lineId, decimal qty)
    {
        EnsureStatus(ShipmentOrderStatus.Draft);
        var line = GetLine(lineId);
        line.Reserve(qty);
    }

    public void Pick(Guid lineId, decimal qty)
    {
        EnsureStatus(ShipmentOrderStatus.InProgress);
        var line = GetLine(lineId);
        line.Pick(qty);
    }

    public void Ship(DateTime shippedAt)
    {
        EnsureStatus(ShipmentOrderStatus.InProgress);

        if (_lines.Count == 0)
            throw new InvalidOperationException("Cannot ship empty order.");

        // Консервативное правило: нельзя отгрузить больше, чем picked.
        // Если захотим частичную отгрузку — сделаем ShipPartially.
        if (_lines.Any(l => l.PickedQuantity < l.OrderedQuantity))
            throw new InvalidOperationException("Cannot ship: not all lines are fully picked.");

        Status = ShipmentOrderStatus.Shipped;
        ShippedAt = shippedAt;
    }

    public void Cancel(DateTime cancelledAt, string? reason = null)
    {
        EnsureStatus(ShipmentOrderStatus.InProgress);

        Status = ShipmentOrderStatus.Cancelled;
        CancelledAt = cancelledAt;
        CancelReason = !string.IsNullOrWhiteSpace(reason) ? reason.Trim() : null;
    }
    
    private ShipmentOrderLine GetLine(Guid lineId) => _lines.FirstOrDefault(x => x.Id.Equals(lineId)) ?? throw new InvalidOperationException($"Line not found: {lineId}");

    private void EnsureStatus(ShipmentOrderStatus status)
    {   
        if (Status != status)
            throw new InvalidOperationException($"Invalid ShipmentOrder status. Expected {status}, actual {Status}.");
    }

}
