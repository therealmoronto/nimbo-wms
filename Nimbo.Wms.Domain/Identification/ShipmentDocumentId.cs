using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct ShipmentDocumentId : IEntityId
{
    public ShipmentDocumentId(Guid value)
    {
        EntityIdExtensions.EnsureNotEmpty<ShipmentDocumentId>(value);
        Value = value;
    }

    public Guid Value { get; }

    public static ShipmentDocumentId New() => EntityIdExtensions.New(id => new ShipmentDocumentId(id));

    public static ShipmentDocumentId From(Guid guid) => EntityIdExtensions.From(guid, id => new ShipmentDocumentId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(ShipmentDocumentId id) => id.Value;
    
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is ShipmentDocumentId id && Value.Equals(id.Value);

    public bool Equals(ShipmentDocumentId other) => Value.Equals(other.Value);
    
    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(ShipmentDocumentId left, ShipmentDocumentId right) => left.Value == right.Value;

    public static bool operator !=(ShipmentDocumentId left, ShipmentDocumentId right) => left.Value != right.Value;
}
