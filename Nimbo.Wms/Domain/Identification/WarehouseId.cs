using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct WarehouseId : IEntityId
{
    public WarehouseId(Guid value)
    {
        EntityId.EnsureNotEmpty<WarehouseId>(value);
        Value = value;
    }
    
    public Guid Value { get; }
    
    public static WarehouseId New() => EntityId.New(id => new WarehouseId(id));
    
    public static WarehouseId From(Guid guid) => EntityId.From(guid, id => new WarehouseId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(WarehouseId id) => id.Value;
}
