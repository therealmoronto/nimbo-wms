using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct CustomerId : IEntityId
{
    public CustomerId(Guid value)
    {
        EntityIdExtensions.EnsureNotEmpty<CustomerId>(value);
        Value = value;
    }
    
    public Guid Value { get; }
    
    public static CustomerId New() => EntityIdExtensions.New(id => new CustomerId(id));
    
    public static CustomerId From(Guid guid) => EntityIdExtensions.From(guid, id => new CustomerId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(CustomerId id) => id.Value;
    
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is CustomerId id && Value.Equals(id.Value);

    public bool Equals(CustomerId other) => Value.Equals(other.Value);
    
    public override int GetHashCode() => Value.GetHashCode();
}
