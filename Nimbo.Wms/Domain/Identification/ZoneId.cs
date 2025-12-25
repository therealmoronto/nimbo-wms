namespace Nimbo.Wms.Domain.Identification;

public readonly struct ZoneId : IEntityId
{
    public ZoneId(Guid value)
    {
        EntityId.EnsureNotEmpty<WarehouseId>(value);
        Value = value;
    }
    
    public Guid Value { get; }
    
    public static ZoneId New() => EntityId.New(id => new ZoneId(id));
    
    public static ZoneId From(Guid guid) => EntityId.From(guid, id => new ZoneId(id));
}
