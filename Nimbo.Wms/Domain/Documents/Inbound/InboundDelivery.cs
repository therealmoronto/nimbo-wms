using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Domain.Documents.Inbound;

public class InboundDelivery : Document<InboundDeliveryId, InboundDeliveryStatus>
{
    private readonly List<InboundDeliveryLine> _lines = new();

    private InboundDelivery()
    {
        // Required by EF Core
    }
    
    public InboundDelivery(
        InboundDeliveryId id,
        DateTime createdAt,
        SupplierId supplierId,
        WarehouseId warehouseId,
        string? externalReference)
        : base(id, InboundDeliveryStatus.Draft, createdAt, externalReference)
    {
        SupplierId = supplierId;
        WarehouseId = warehouseId;
    }

    public SupplierId SupplierId { get; private set; }

    public WarehouseId WarehouseId { get; private set; }
    
    public DateTime? StartedAt { get; private set; }

    public DateTime? ReceivedAt { get; private set; }

    public IReadOnlyCollection<InboundDeliveryLine> Lines => _lines;

    public InboundDeliveryLine AddLine(ItemId itemId, decimal expectedQuantity, UnitOfMeasure uom, string? batchNumber = null, DateTime? expiryDate = null)
    {
        EnsureStatus(InboundDeliveryStatus.Draft);

        var line = new InboundDeliveryLine(Id, itemId, expectedQuantity, uom, batchNumber, expiryDate);
        _lines.Add(line);
        return line;
    }

    public void Start()
    {
        StartedAt = DateTime.UtcNow;
        EnsureStatus(InboundDeliveryStatus.Draft);
        Status = InboundDeliveryStatus.InProgress;
    }

    public void ReceiveLine(
        Guid lineId,
        decimal receivedQuantity,
        string? batchNumber,
        DateTime? expiryDate,
        bool allowOverReceipt,
        bool batchRequired)
    {
        EnsureStatus(InboundDeliveryStatus.InProgress);

        var line = _lines.SingleOrDefault(x => x.Id == lineId)
                   ?? throw new KeyNotFoundException("Inbound delivery line not found.");

        line.Receive(receivedQuantity, batchNumber, expiryDate, allowOverReceipt, batchRequired);
    }

    public void Complete(DateTime utcNow)
    {
        EnsureStatus(InboundDeliveryStatus.InProgress);

        if (_lines.Count == 0)
            throw new InvalidOperationException("Inbound delivery must have at least one line.");

        if (_lines.Any(l => !l.IsReceived))
            throw new InvalidOperationException("All lines must be received before completion.");

        // Determine final status: ReceivedFully vs. ReceivedPartially
        var fully = _lines.All(l => l.ReceivedQuantity == l.ExpectedQuantity);
        Status = fully ? InboundDeliveryStatus.ReceivedFully : InboundDeliveryStatus.ReceivedPartially;

        ReceivedAt = utcNow; // receiving timestamp recorded
    }

    private void EnsureStatus(InboundDeliveryStatus status)
    {
        if (Status != status)
            throw new InvalidOperationException("Inbound delivery is closed for editing.");
    }
}
