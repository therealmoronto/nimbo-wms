namespace Nimbo.Wms.Domain.Identification;

public readonly struct InboundDeliveryId : IEntityId
{
    public InboundDeliveryId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("InboundDeliveryId cannot be empty.", nameof(value));

        Value = value;
    }

    public Guid Value { get; }

    public static InboundDeliveryId New() => EntityId.New(id => new InboundDeliveryId(id));

    public static InboundDeliveryId From(Guid guid) => EntityId.From(guid, id => new InboundDeliveryId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(InboundDeliveryId id) => id.Value;
}
