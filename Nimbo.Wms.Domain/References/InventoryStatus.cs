using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.References;

/// <summary>
/// Inventory statuses for <see cref="InventoryItem"/>
/// For more information see https://github.com/therealmoronto/nimbo-wms/wiki/inventory-statuses
/// </summary>
[PublicAPI]
public enum InventoryStatus
{
    Available = 1,
    Reserved,
    Picked,
    InTransit,
    Hold,
    Damaged,
    Expired,
    Audit,
}
