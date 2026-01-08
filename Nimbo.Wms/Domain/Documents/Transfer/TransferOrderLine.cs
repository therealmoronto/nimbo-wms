using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Documents.Transfer;

public class TransferOrderLine
{
    private TransferOrderLine() { }
    
    public TransferOrderLine(TransferOrderId documentId, ItemId itemId, Quantity plannedQuantity)
    {
        if (plannedQuantity.Value <= 0m)
            throw new ArgumentOutOfRangeException(nameof(plannedQuantity), "Planned quantity must be > 0.");

        TransferOrderId = documentId;
        ItemId = itemId;
        PlannedQuantity = plannedQuantity;

        PickedQuantity = new Quantity(0m, plannedQuantity.Uom);
        ReceivedQuantity = new Quantity(0m, plannedQuantity.Uom);
    }
    
    public TransferOrderId TransferOrderId { get; }

    public Guid Id { get; } = Guid.NewGuid();

    public ItemId ItemId { get; }

    public Quantity PlannedQuantity { get; }

    public Quantity PickedQuantity { get; private set; }

    public Quantity ReceivedQuantity { get; private set; }

    public void AddPicked(Quantity qty)
    {
        EnsureSameUom(qty, PlannedQuantity);

        if (qty.Value <= 0m)
            throw new ArgumentOutOfRangeException(nameof(qty), "Picked quantity must be > 0.");

        var newValue = PickedQuantity.Value + qty.Value;
        if (newValue > PlannedQuantity.Value)
            throw new InvalidOperationException("Cannot pick more than planned.");

        PickedQuantity = PickedQuantity with { Value = newValue };
    }

    public void AddReceived(Quantity qty)
    {
        EnsureSameUom(qty, PlannedQuantity);

        if (qty.Value <= 0m)
            throw new ArgumentOutOfRangeException(nameof(qty), "Received quantity must be > 0.");

        var newValue = ReceivedQuantity.Value + qty.Value;
        if (newValue > PickedQuantity.Value)
            throw new InvalidOperationException("Cannot receive more than picked/shipped.");

        ReceivedQuantity = ReceivedQuantity with { Value = newValue };
    }

    public bool IsFullyReceived => ReceivedQuantity.Value == PickedQuantity.Value;

    private static void EnsureSameUom(Quantity a, Quantity b)
    {
        if (a.Uom != b.Uom)
            throw new InvalidOperationException($"UoM mismatch: {a.Uom} vs {b.Uom}");
    }
}
