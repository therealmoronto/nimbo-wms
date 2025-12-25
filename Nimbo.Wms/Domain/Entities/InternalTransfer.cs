using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Domain.Entities;

public class InternalTransfer : IEntity<InternalTransferId>
{
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

    private static Quantity EnsurePositive(Quantity quantity)
    {
        // Wiki: quantity must be > 0 :contentReference[oaicite:2]{index=2}
        if (quantity.Value <= 0m)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        return quantity;
    }

    private static string? TrimOrNull(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    
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

}
