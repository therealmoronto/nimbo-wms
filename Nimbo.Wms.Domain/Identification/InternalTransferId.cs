using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct InternalTransferId : IEntityId
{
    public InternalTransferId(Guid value)
    {
        EntityIdExtensions.EnsureNotEmpty<InternalTransferId>(value);
        Value = value;
    }

    public Guid Value { get; }

    public static InternalTransferId New() => EntityIdExtensions.New(id => new InternalTransferId(id));

    public static InternalTransferId From(Guid guid) => EntityIdExtensions.From(guid, id => new InternalTransferId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(InternalTransferId id) => id.Value;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is InternalTransferId id && Value.Equals(id.Value);

    public bool Equals(InternalTransferId other) => Value.Equals(other.Value);
    
    public override int GetHashCode() => Value.GetHashCode();
    
    public static bool operator ==(InternalTransferId left, InternalTransferId right) => left.Value == right.Value;
    
    public static bool operator !=(InternalTransferId left, InternalTransferId right) => left.Value != right.Value;
}
