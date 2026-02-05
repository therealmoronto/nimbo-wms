using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct InventoryItemId : IEntityId
{
    public InventoryItemId(Guid value)
    {
        EntityIdExtensions.EnsureNotEmpty<InventoryItemId>(value);
        Value = value;
    }
    
    public Guid Value { get; }

    public static InventoryItemId New() => EntityIdExtensions.New(id => new InventoryItemId(id));
    
    public static InventoryItemId From(Guid guid) => EntityIdExtensions.From(guid, id => new InventoryItemId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(InventoryItemId id) => id.Value;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is InventoryItemId id && Value.Equals(id.Value);

    public bool Equals(InventoryItemId other) => Value.Equals(other.Value);
    
    public override int GetHashCode() => Value.GetHashCode();
}
