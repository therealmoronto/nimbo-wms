using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct LocationId : IEntityId
{
    public LocationId(Guid value)
    {
        EntityIdExtensions.EnsureNotEmpty<LocationId>(value);
        Value = value;
    }
    
    public Guid Value { get; }

    public static LocationId New() => EntityIdExtensions.New(id => new LocationId(id));
    
    public static LocationId From(Guid guid) => EntityIdExtensions.From(guid, id => new LocationId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(LocationId id) => id.Value;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is LocationId id && Value.Equals(id.Value);

    public bool Equals(LocationId other) => Value.Equals(other.Value);
    
    public override int GetHashCode() => Value.GetHashCode();
}
