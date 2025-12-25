using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Documents.Audit;

public class InventoryCountLine
{
    public InventoryCountLine(ItemId itemId, LocationId locationId, Quantity systemQuantity)
    {
        if (systemQuantity.Value < 0m)
            throw new ArgumentOutOfRangeException(nameof(systemQuantity), "SystemQuantity cannot be negative.");

        ItemId = itemId;
        LocationId = locationId;
        SystemQuantity = systemQuantity;
    }
    
    public Guid Id { get; } = Guid.NewGuid();

    public ItemId ItemId { get; }

    public LocationId LocationId { get; }

    public Quantity SystemQuantity { get; }

    public Quantity? CountedQuantity { get; private set; }

    public bool IsCounted => CountedQuantity != null;
    
    public void SetCountedQuantity(Quantity countedQuantity)
    {
        if (countedQuantity.Value < 0m)
            throw new ArgumentOutOfRangeException(nameof(countedQuantity), "CountedQuantity must be >= 0.");

        if (countedQuantity.Uom != SystemQuantity.Uom)
            throw new InvalidOperationException($"UoM mismatch: System={SystemQuantity.Uom}, Counted={countedQuantity.Uom}");

        CountedQuantity = countedQuantity;
    }

    public bool HasDiscrepancy() => !IsCounted || CountedQuantity!.Value != SystemQuantity.Value;
}
