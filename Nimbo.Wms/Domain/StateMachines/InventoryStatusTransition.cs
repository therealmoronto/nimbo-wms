using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.References;

/// <summary>
/// Allowed transitions for InventoryStatus
/// Keep this in sync with the Inventory Status State Machine diagram.
/// For more information see https://github.com/therealmoronto/nimbo-wms/wiki/Inventory-Status-State-Machine
/// </summary>
// TODO Create unit tests for this
[PublicAPI]
public static class InventoryStatusTransition
{
    private static readonly IReadOnlyDictionary<InventoryStatus, InventoryStatus[]> AllowedTransitions =
        new Dictionary<InventoryStatus, InventoryStatus[]>
        {
            { InventoryStatus.Available, [InventoryStatus.Reserved, InventoryStatus.Picked, InventoryStatus.InTransit, InventoryStatus.Hold, InventoryStatus.Audit, InventoryStatus.Damaged, InventoryStatus.Expired] },
            { InventoryStatus.Reserved, [InventoryStatus.Picked, InventoryStatus.Available, InventoryStatus.Audit] },
            { InventoryStatus.Picked, [InventoryStatus.Available, InventoryStatus.InTransit, InventoryStatus.Damaged] },
            { InventoryStatus.InTransit, [InventoryStatus.Available, InventoryStatus.Damaged, InventoryStatus.Audit] },
            { InventoryStatus.Hold, [InventoryStatus.Available, InventoryStatus.Audit, InventoryStatus.Damaged] },
            { InventoryStatus.Audit, [InventoryStatus.Available, InventoryStatus.Hold, InventoryStatus.Damaged, InventoryStatus.Expired] },
            { InventoryStatus.Damaged, [InventoryStatus.Audit]},
            { InventoryStatus.Expired, [InventoryStatus.Audit]},
        };
    
    public static bool CanTransition(InventoryStatus from, InventoryStatus to) => from == to || (AllowedTransitions.TryGetValue(from, out var allowed) && allowed.Contains(to));
    
    public static void EnsureCanTransition(InventoryStatus from, InventoryStatus to)
    {
        if (!CanTransition(from, to))
            throw new InvalidOperationException($"InventoryStatus '{from}' cannot transition to '{to}'");
    }
}
