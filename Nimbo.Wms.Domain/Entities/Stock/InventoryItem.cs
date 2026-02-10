using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Entities.Stock;

public class InventoryItem : BaseEntity<InventoryItemId>
{
    // ReSharper disable once UnusedMember.Local
    private InventoryItem()
    {
        // Required by EF Core
    }
    
    /// <exception cref="ArgumentException">Thrown when the provided strings of batchNumber or serialNumber are empty or whitespace or when quantity is negative</exception>
    public InventoryItem(
        InventoryItemId id,
        ItemId itemId,
        WarehouseId warehouseId,
        LocationId locationId,
        Quantity quantity,
        InventoryStatus status = InventoryStatus.Available,
        BatchId? batchId = null,
        string? serialNumber = null,
        decimal? unitCost = null)
    {
        Id = id;

        ItemId = itemId;
        WarehouseId = warehouseId;
        LocationId = locationId;

        BatchId = batchId;
        SerialNumber = TrimOrNull(serialNumber);

        Quantity = quantity;
        EnsureSerialRules(SerialNumber, Quantity);

        Status = status;

        UnitCost = RequireNonNegativeOrNull(unitCost, nameof(unitCost));
    }

    public ItemId ItemId { get; }
    
    public WarehouseId WarehouseId { get; private set; }
    
    public LocationId LocationId { get; private set; }

    /// <summary>
    /// Current stock quantity expressed in a unit of measure.
    /// Usually equals Item.BaseUom.
    /// </summary>
    public Quantity Quantity { get; private set; } = null!;

    /// <summary>
    /// Optional batch/lot identifier stored directly on InventoryItem (MVP).
    /// If you later rely on Batch entity, you can use BatchId instead or keep both.
    /// </summary>
    public BatchId? BatchId { get; private set; }

    /// <summary>
    /// Optional serial number (when used, quantity is typically 1).
    /// </summary>
    public string? SerialNumber { get; private set; }

    public InventoryStatus Status { get; private set; }

    public decimal? UnitCost { get; private set; }
    
    public void Increase(Quantity amount)
    {
        EnsureSameUom(amount);
        Quantity += amount;
    }

    public void Decrease(Quantity amount)
    {
        EnsureSameUom(amount);
        Quantity -= amount; // Quantity.Subtract already prevents negative results
    }

    public void MoveWithinWarehouse(LocationId toLocationId)
    {
        // Domain rule "Location belongs to same Warehouse" is validated in application layer,
        // because it needs Location/Warehouse context. 
        LocationId = toLocationId;
    }

    public void MoveToWarehouse(WarehouseId toWarehouseId, LocationId toLocationId)
    {
        // For inter-warehouse transfers, processes usually set status to InTransit.
        WarehouseId = toWarehouseId;
        LocationId = toLocationId;
    }

    public void ChangeStatus(InventoryStatus newStatus)
    {
        InventoryStatusTransition.EnsureCanTransition(Status, newStatus);
        Status = newStatus;
    }

    public void Reserve()
    {
        EnsureReservable();
        ChangeStatus(InventoryStatus.Reserved);
    }

    public void Unreserve() => ChangeStatus(InventoryStatus.Available);

    public void MarkPicked() => ChangeStatus(InventoryStatus.Picked);

    public void MarkInTransit() => ChangeStatus(InventoryStatus.InTransit);

    public void PutOnHold() => ChangeStatus(InventoryStatus.Hold);

    public void MarkDamaged() => ChangeStatus(InventoryStatus.Damaged);

    public void MarkExpired() => ChangeStatus(InventoryStatus.Expired);

    public void StartAudit() => ChangeStatus(InventoryStatus.Audit);

    public void SetBatchNumber(BatchId? batchId) => BatchId = batchId;

    public void SetSerialNumber(string? serialNumber)
    {
        SerialNumber = TrimOrNull(serialNumber);
        EnsureSerialRules(SerialNumber, Quantity);
    }

    public void SetUnitCost(decimal? unitCost)
        => UnitCost = RequireNonNegativeOrNull(unitCost, nameof(unitCost));

    private void EnsureReservable()
    {
        // Wiki: Damaged/Expired/Hold/Audit must not be reservable. 
        if (Status is InventoryStatus.Damaged
            or InventoryStatus.Expired
            or InventoryStatus.Hold
            or InventoryStatus.Audit)
        {
            throw new InvalidOperationException($"Stock in status {Status} cannot be reserved.");
        }
    }

    private void EnsureSameUom(Quantity amount)
    {
        if (Quantity.Uom != amount.Uom)
            throw new InvalidOperationException($"UoM mismatch: {Quantity.Uom} vs {amount.Uom}");
    }

    private static void EnsureSerialRules(string? serialNumber, Quantity quantity)
    {
        // Wiki: SerialNumber implies quantity control (typically one unit per record). 
        if (serialNumber is not null && quantity.Value != 1m)
            throw new InvalidOperationException("Serialized stock must have quantity = 1.");
    }

    private static string? TrimOrNull(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static decimal? RequireNonNegativeOrNull(decimal? value, string paramName)
    {
        if (value is not null && value.Value < 0m)
            throw new ArgumentOutOfRangeException(paramName, "Value cannot be negative.");
        return value;
    }
}
