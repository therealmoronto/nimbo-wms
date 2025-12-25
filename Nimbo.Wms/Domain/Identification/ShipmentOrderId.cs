namespace Nimbo.Wms.Domain.Identification;

public readonly struct ShipmentOrderId : IEntityId
{
    public ShipmentOrderId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("ShipmentOrderId cannot be empty.", nameof(value));

        Value = value;
    }

    public Guid Value { get; }

    public static ShipmentOrderId New() => EntityId.New(id => new ShipmentOrderId(id));

    public static ShipmentOrderId From(Guid guid) => EntityId.From(guid, id => new ShipmentOrderId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(ShipmentOrderId id) => id.Value;
}
