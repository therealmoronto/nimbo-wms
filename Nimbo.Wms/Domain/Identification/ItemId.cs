using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct ItemId : IEntityId
{
    public ItemId(Guid value)
    {
        EntityId.EnsureNotEmpty<ItemId>(value);
        Value = value;
    }
    
    public Guid Value { get; }

    public static ItemId New() => EntityId.New(id => new ItemId(id));
    
    public static ItemId From(Guid guid) => EntityId.From(guid, id => new ItemId(id));
}
