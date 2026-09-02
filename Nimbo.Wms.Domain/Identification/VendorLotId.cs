using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct VendorLotId : IEntityId
{
    public VendorLotId(Guid value)
    {
        EntityIdExtensions.EnsureNotEmpty<VendorLotId>(value);
        Value = value;
    }

    public Guid Value { get; }

    public static VendorLotId New() => EntityIdExtensions.New(id => new VendorLotId(id));

    public static VendorLotId From(Guid guid) => EntityIdExtensions.From(guid, id => new VendorLotId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(VendorLotId id) => id.Value;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is VendorLotId id && Value.Equals(id.Value);

    public bool Equals(VendorLotId other) => Value.Equals(other.Value);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(VendorLotId left, VendorLotId right) => left.Value == right.Value;

    public static bool operator !=(VendorLotId left, VendorLotId right) => left.Value != right.Value;
}
