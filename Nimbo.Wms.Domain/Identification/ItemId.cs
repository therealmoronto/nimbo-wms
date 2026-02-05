using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct ItemId : IEntityId
{
    public ItemId(Guid value)
    {
        EntityIdExtensions.EnsureNotEmpty<ItemId>(value);
        Value = value;
    }
    
    public Guid Value { get; }

    public static ItemId New() => EntityIdExtensions.New(id => new ItemId(id));
    
    public static ItemId From(Guid guid) => EntityIdExtensions.From(guid, id => new ItemId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(ItemId id) => id.Value;
    
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is ItemId id && Value.Equals(id.Value);

    public bool Equals(ItemId other) => Value.Equals(other.Value);
    
    public override int GetHashCode() => Value.GetHashCode();
}
