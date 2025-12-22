using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct LocationId : IEntityId
{
    public LocationId(Guid value)
    {
        EntityId.EnsureNotEmpty<LocationId>(value);
        Value = value;
    }
    
    public Guid Value { get; }

    public static LocationId New() => EntityId.New(id => new LocationId(id));
    
    public static LocationId From(Guid guid) => EntityId.From(guid, id => new LocationId(id));
}
