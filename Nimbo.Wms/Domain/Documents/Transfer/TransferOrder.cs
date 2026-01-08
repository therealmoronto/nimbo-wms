using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Documents.Transfer;

public class TransferOrder : Document<TransferOrderId, TransferOrderStatus>
{
    private readonly List<TransferOrderLine> _lines = new();

    public WarehouseId FromWarehouseId { get; }
    public WarehouseId ToWarehouseId { get; }

    public IReadOnlyCollection<TransferOrderLine> Lines => _lines.AsReadOnly();

    public DateTime? PickingStartedAt { get; private set; }
    public DateTime? ShippedAt { get; private set; }
    public DateTime? ReceivedAt { get; private set; }

    public TransferOrder(
        TransferOrderId id,
        DateTime createdAt,
        WarehouseId fromWarehouseId,
        WarehouseId toWarehouseId,
        string? externalReference = null)
        : base(id, TransferOrderStatus.Draft, createdAt, externalReference)
    {
        if (fromWarehouseId.Value == toWarehouseId.Value)
            throw new ArgumentException("FromWarehouseId and ToWarehouseId cannot be the same.");

        FromWarehouseId = fromWarehouseId;
        ToWarehouseId = toWarehouseId;
    }

    public static TransferOrder Create(
        WarehouseId fromWarehouseId,
        WarehouseId toWarehouseId,
        DateTime createdAt,
        string? externalReference = null)
        => new(TransferOrderId.New(), createdAt, fromWarehouseId, toWarehouseId, externalReference);

    public TransferOrderLine AddLine(ItemId itemId, Quantity plannedQuantity)
    {
        EnsureStatus(TransferOrderStatus.Draft);

        var line = new TransferOrderLine(Id, itemId, plannedQuantity);
        _lines.Add(line);
        return line;
    }

    public void StartPicking(DateTime utcNow)
    {
        EnsureStatus(TransferOrderStatus.Draft);

        if (_lines.Count == 0)
            throw new InvalidOperationException("TransferOrder must have at least one line.");

        Status = TransferOrderStatus.Picking;
        PickingStartedAt = utcNow;
    }

    public void RecordPicked(Guid lineId, Quantity pickedQty)
    {
        EnsureStatus(TransferOrderStatus.Picking);
        GetLine(lineId).AddPicked(pickedQty);
    }

    public void Ship(DateTime utcNow)
    {
        EnsureStatus(TransferOrderStatus.Picking);

        // Минимальное правило: нельзя отправить совсем пустое (ничего не picked).
        if (_lines.All(l => l.PickedQuantity.Value == 0m))
            throw new InvalidOperationException("Cannot ship: nothing was picked.");

        Status = TransferOrderStatus.InTransit;
        ShippedAt = utcNow;
    }

    public void RecordReceived(Guid lineId, Quantity receivedQty)
    {
        EnsureStatus(TransferOrderStatus.InTransit);
        GetLine(lineId).AddReceived(receivedQty);
    }

    public void CompleteReceiving(DateTime utcNow)
    {
        EnsureStatus(TransferOrderStatus.InTransit);

        // Нельзя закрыть, если по всем линиям received = 0 (защитимся от случайного клика).
        if (_lines.All(l => l.ReceivedQuantity.Value == 0m))
            throw new InvalidOperationException("Cannot complete receiving: nothing was received.");

        var fullyReceived = _lines.All(l => l.IsFullyReceived);

        Status = fullyReceived ? TransferOrderStatus.ReceivedFully : TransferOrderStatus.ReceivedPartially;
        ReceivedAt = utcNow;
    }

    private TransferOrderLine GetLine(Guid id) => _lines.FirstOrDefault(x => x.Id == id) ?? throw new KeyNotFoundException("TransferOrderLine not found.");

    private void EnsureStatus(TransferOrderStatus expected)
    {
        if (Status != expected)
            throw new InvalidOperationException($"Invalid TransferOrder status. Expected {expected}, actual {Status}.");
    }
}
