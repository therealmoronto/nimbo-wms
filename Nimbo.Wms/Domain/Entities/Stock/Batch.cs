using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Domain.Entities.Stock;

public class Batch : IEntity<BatchId>
{
    // ReSharper disable once UnusedMember.Global
    public Batch()
    {
        // Required by EF Core
    }

    /// <exception cref="ArgumentException">Thrown when the provided strings of batchNumber are empty or whitespace or when expiryDate is earlier than manufacturedAt</exception>
    public Batch(
        BatchId id,
        ItemId itemId,
        string batchNumber,
        SupplierId? supplierId = null,
        DateTime? manufacturedAt = null,
        DateTime? expiryDate = null,
        DateTime? receivedAt = null,
        string? notes = null)
    {
        Id = id;

        ItemId = itemId;

        BatchNumber = RequireNonEmpty(batchNumber, nameof(batchNumber));

        SupplierId = supplierId;

        ManufacturedAt = manufacturedAt;
        ExpiryDate = expiryDate;
        ReceivedAt = receivedAt;

        EnsureDatesValid(ManufacturedAt, ExpiryDate);

        Notes = TrimOrNull(notes);
    }
    
    public BatchId Id { get; }

    public ItemId ItemId { get; }

    public string BatchNumber { get; private set; }

    public SupplierId? SupplierId { get; private set; }

    public DateTime? ManufacturedAt { get; private set; }

    public DateTime? ExpiryDate { get; private set; }

    public DateTime? ReceivedAt { get; private set; }

    public string? Notes { get; private set; }
    
    public void ChangeBatchNumber(string batchNumber) => BatchNumber = RequireNonEmpty(batchNumber, nameof(batchNumber));

    public void SetSupplier(SupplierId? supplierId) => SupplierId = supplierId;

    public void SetManufacturedAt(DateTime? manufacturedAt)
    {
        EnsureDatesValid(manufacturedAt, ExpiryDate);
        ManufacturedAt = manufacturedAt;
    }

    public void SetExpiryDate(DateTime? expiryDate)
    {
        EnsureDatesValid(ManufacturedAt, expiryDate);
        ExpiryDate = expiryDate;
    }

    public void SetReceivedAt(DateTime? receivedAt) => ReceivedAt = receivedAt;

    public void SetNotes(string? notes) => Notes = TrimOrNull(notes);

    private static void EnsureDatesValid(DateTime? manufacturedAt, DateTime? expiryDate)
    {
        if (manufacturedAt is not null && expiryDate is not null && expiryDate.Value <= manufacturedAt.Value)
            throw new ArgumentException("ExpiryDate must be later than ManufacturedAt.");
    }

    private static string RequireNonEmpty(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be empty.", paramName);

        return value.Trim();
    }

    private static string? TrimOrNull(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
