using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct SupplierItemId : IEntityId, IEquatable<SupplierItemId>
{
    public SupplierItemId(Guid value)
    {
        EntityIdExtensions.EnsureNotEmpty<SupplierItemId>(value);
        Value = value;
    }
    
    public Guid Value { get; }

    public static SupplierItemId New() => EntityIdExtensions.New(id => new SupplierItemId(id));
    
    public static SupplierItemId From(Guid guid) => EntityIdExtensions.From(guid, id => new SupplierItemId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(SupplierItemId id) => id.Value;
    
    public override bool Equals(object? obj) => obj is SupplierItemId id && Value.Equals(id.Value);

    public bool Equals(SupplierItemId other) => Value.Equals(other.Value);

    public override int GetHashCode() => Value.GetHashCode();
}
