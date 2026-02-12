using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct ShipmentOrderId : IEntityId, IEquatable<ShipmentOrderId>
{
    public ShipmentOrderId(Guid value)
    {
        EntityIdExtensions.EnsureNotEmpty<ShipmentOrderId>(value);
        Value = value;
    }

    public Guid Value { get; }

    public static ShipmentOrderId New() => EntityIdExtensions.New(id => new ShipmentOrderId(id));

    public static ShipmentOrderId From(Guid guid) => EntityIdExtensions.From(guid, id => new ShipmentOrderId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(ShipmentOrderId id) => id.Value;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is ShipmentOrderId id && Value.Equals(id.Value);

    public bool Equals(ShipmentOrderId other) => Value.Equals(other.Value);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(ShipmentOrderId left, ShipmentOrderId right) => left.Value == right.Value;

    public static bool operator !=(ShipmentOrderId left, ShipmentOrderId right) => left.Value != right.Value;
}
