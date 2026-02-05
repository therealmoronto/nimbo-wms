using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct InboundDeliveryId : IEntityId
{
    public InboundDeliveryId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("InboundDeliveryId cannot be empty.", nameof(value));

        Value = value;
    }

    public Guid Value { get; }

    public static InboundDeliveryId New() => EntityIdExtensions.New(id => new InboundDeliveryId(id));

    public static InboundDeliveryId From(Guid guid) => EntityIdExtensions.From(guid, id => new InboundDeliveryId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(InboundDeliveryId id) => id.Value;
    
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is InboundDeliveryId id && Value.Equals(id.Value);

    public bool Equals(InboundDeliveryId other) => Value.Equals(other.Value);
    
    public override int GetHashCode() => Value.GetHashCode();
}
