namespace Nimbo.Wms.Domain.Identification;

public readonly struct InventoryCountId : IEntityId
{
    public InventoryCountId(Guid value)
    {
        EntityId.EnsureNotEmpty<InventoryCountId>(value);
        Value = value;
    }

    public Guid Value { get; }

    public static InventoryCountId New() => EntityId.New(id => new InventoryCountId(id));
    public static InventoryCountId From(Guid guid) => EntityId.From(guid, id => new InventoryCountId(id));
}
