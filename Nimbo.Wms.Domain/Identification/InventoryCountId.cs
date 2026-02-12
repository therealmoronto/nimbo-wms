using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct InventoryCountId : IEntityId
{
    public InventoryCountId(Guid value)
    {
        EntityIdExtensions.EnsureNotEmpty<InventoryCountId>(value);
        Value = value;
    }

    public Guid Value { get; }

    public static InventoryCountId New() => EntityIdExtensions.New(id => new InventoryCountId(id));

    public static InventoryCountId From(Guid guid) => EntityIdExtensions.From(guid, id => new InventoryCountId(id));
    
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is InventoryCountId id && Value.Equals(id.Value);

    public bool Equals(InventoryCountId other) => Value.Equals(other.Value);
    
    public override int GetHashCode() => Value.GetHashCode();
    
    public static bool operator ==(InventoryCountId left, InventoryCountId right) => left.Value == right.Value;
    
    public static bool operator !=(InventoryCountId left, InventoryCountId right) => left.Value != right.Value;
}
