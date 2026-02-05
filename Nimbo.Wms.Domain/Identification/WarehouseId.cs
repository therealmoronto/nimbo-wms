using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct WarehouseId : IEntityId, IEquatable<WarehouseId>
{
    public WarehouseId(Guid value)
    {
        EntityIdExtensions.EnsureNotEmpty<WarehouseId>(value);
        Value = value;
    }

    public Guid Value { get; }

    public static WarehouseId New() => EntityIdExtensions.New(id => new WarehouseId(id));

    public static WarehouseId From(Guid guid) => EntityIdExtensions.From(guid, id => new WarehouseId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(WarehouseId id) => id.Value;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is WarehouseId id && Value.Equals(id.Value);

    public bool Equals(WarehouseId other) => Value.Equals(other.Value);

    public override int GetHashCode() => Value.GetHashCode();
}
