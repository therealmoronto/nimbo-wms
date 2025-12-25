using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Entities.Movements;

public class InternalTransfer : IEntity<InternalTransferId>
{
    private InternalTransfer()
    {
        // Required by EF Core
    }

    /// <exception cref="ArgumentOutOfRangeException">Quantity must be greater than zero.</exception>
    public InternalTransfer(
        InternalTransferId id,
        WarehouseId warehouseId,
        ItemId itemId,
        LocationId fromLocationId,
        LocationId toLocationId,
        Quantity quantity,
        DateTime occurredAt,
        string? reason = null)
    {
        Id = id;

        WarehouseId = warehouseId;
        ItemId = itemId;

        FromLocationId = fromLocationId;
        ToLocationId = toLocationId;

        if (FromLocationId.Value == ToLocationId.Value)
            throw new ArgumentException("FromLocationId and ToLocationId cannot be the same.");

        Quantity = EnsurePositive(quantity);

        OccurredAt = occurredAt;

        Reason = TrimOrNull(reason);
    }

    public InternalTransferId Id { get; }

    public WarehouseId WarehouseId { get; }
    
    public ItemId ItemId { get; }

    public LocationId FromLocationId { get; }
    
    public LocationId ToLocationId { get; }

    public Quantity Quantity { get; }

    public DateTime OccurredAt { get; }

    /// <summary>
    /// Optional reason (Putaway, Relocation, Consolidation, etc.).
    /// </summary>
    public string? Reason { get; }

    /// <exception cref="ArgumentOutOfRangeException">Quantity must be greater than zero.</exception>
    private static Quantity EnsurePositive(Quantity quantity)
    {
        if (quantity.Value <= 0m)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");

        return quantity;
    }

    private static string? TrimOrNull(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
