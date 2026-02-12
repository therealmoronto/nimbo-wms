using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct ZoneId : IEntityId
{
    public ZoneId(Guid value)
    {
        EntityIdExtensions.EnsureNotEmpty<ZoneId>(value);
        Value = value;
    }
    
    public Guid Value { get; }
    
    public static ZoneId New() => EntityIdExtensions.New(id => new ZoneId(id));
    
    public static ZoneId From(Guid guid) => EntityIdExtensions.From(guid, id => new ZoneId(id));

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is ZoneId id && Value.Equals(id.Value);
    
    public bool Equals(ZoneId other) => Value.Equals(other.Value);
    
    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(ZoneId left, ZoneId right) => left.Value == right.Value;

    public static bool operator !=(ZoneId left, ZoneId right) => left.Value != right.Value;
}
