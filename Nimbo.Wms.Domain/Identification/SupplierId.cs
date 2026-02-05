using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct SupplierId : IEntityId, IEquatable<SupplierId>
{
    public SupplierId(Guid value)
    {
        EntityIdExtensions.EnsureNotEmpty<SupplierId>(value);
        Value = value;
    }
    
    public Guid Value { get; }

    public static SupplierId New() => EntityIdExtensions.New(id => new SupplierId(id));
    
    public static SupplierId From(Guid guid) => EntityIdExtensions.From(guid, id => new SupplierId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(SupplierId id) => id.Value;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is SupplierId id && Value.Equals(id.Value);

    public bool Equals(SupplierId other) => Value.Equals(other.Value);

    public override int GetHashCode() => Value.GetHashCode();
}
