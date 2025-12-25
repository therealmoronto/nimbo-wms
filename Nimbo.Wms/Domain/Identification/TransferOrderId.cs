namespace Nimbo.Wms.Domain.Identification;

public readonly struct TransferOrderId : IEntityId
{
    public TransferOrderId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("TransferOrderId cannot be empty.", nameof(value));

        Value = value;
    }

    public Guid Value { get; }

    public static TransferOrderId New() => EntityId.New(id => new TransferOrderId(id));

    public static TransferOrderId From(Guid guid) => EntityId.From(guid, id => new TransferOrderId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(TransferOrderId id) => id.Value;
}
