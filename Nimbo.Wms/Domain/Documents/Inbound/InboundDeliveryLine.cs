using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Domain.Documents.Inbound;

public sealed class InboundDeliveryLine
{
    private InboundDeliveryLine()
    {
        // Required by EF Core
    } 
    
    public InboundDeliveryLine(InboundDeliveryId documentId, ItemId itemId, decimal expectedQuantity, UnitOfMeasure uom, string? batchNumber = null, DateTime? expiryDate = null)
    {
        if (expectedQuantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(expectedQuantity), "Expected quantity must be > 0.");

        InboundDeliveryId = documentId;
        ItemId = itemId;
        ExpectedQuantity = expectedQuantity;
        Uom = uom;

        BatchNumber = batchNumber;
        ExpiryDate = expiryDate;
    }

    public Guid Id { get; } = Guid.NewGuid();
    
    public InboundDeliveryId InboundDeliveryId { get; }

    public ItemId ItemId { get; }

    public decimal ExpectedQuantity { get; }

    public UnitOfMeasure Uom { get; }

    public decimal? ReceivedQuantity { get; private set; }

    public string? BatchNumber { get; private set; }

    public DateTime? ExpiryDate { get; private set; }

    public bool IsReceived => ReceivedQuantity.HasValue;

    /// <summary>
    /// Set received quantity and batch number.
    /// </summary>
    /// <param name="receivedQuantity">Received quantity.</param>
    /// <param name="batchNumber">Batch number.</param>
    /// <param name="expiryDate">Expiry date.</param>
    /// <param name="allowOverReceipt">Allow over-receipt.</param>
    /// <param name="batchRequired">Batch required.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when received quantity is negative.</exception>
    /// <exception cref="InvalidOperationException">Thrown when over-receipt is not allowed or batch number is required but missing.</exception>
    public void Receive(
        decimal receivedQuantity,
        string? batchNumber,
        DateTime? expiryDate,
        bool allowOverReceipt,
        bool batchRequired)
    {
        if (receivedQuantity < 0)
            throw new ArgumentOutOfRangeException(nameof(receivedQuantity), "Received quantity must be >= 0.");

        if (!allowOverReceipt && receivedQuantity > ExpectedQuantity)
            throw new InvalidOperationException("Over-receipt is not allowed.");

        if (batchRequired && string.IsNullOrWhiteSpace(batchNumber))
            throw new InvalidOperationException("BatchNumber is required for batch-controlled items."); 

        ReceivedQuantity = receivedQuantity;
        BatchNumber = batchNumber;
        ExpiryDate = expiryDate;
    }
}
