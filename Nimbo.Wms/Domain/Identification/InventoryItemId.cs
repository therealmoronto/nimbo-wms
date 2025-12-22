using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct InventoryItemId : IEntityId
{
    public InventoryItemId(Guid value)
    {
        EntityId.EnsureNotEmpty<InventoryItemId>(value);
        Value = value;
    }
    
    public Guid Value { get; }

    public static InventoryItemId New() => EntityId.New(id => new InventoryItemId(id));
    
    public static InventoryItemId From(Guid guid) => EntityId.From(guid, id => new InventoryItemId(id));
}
