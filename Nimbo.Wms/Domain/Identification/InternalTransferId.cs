namespace Nimbo.Wms.Domain.Identification;

public readonly struct InternalTransferId : IEntityId
{
    public InternalTransferId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("InternalTransferId cannot be empty.", nameof(value));

        Value = value;
    }

    public Guid Value { get; }

    public static InternalTransferId New() => EntityId.New(id => new InternalTransferId(id));

    public static InternalTransferId From(Guid guid) => EntityId.From(guid, id => new InternalTransferId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(InternalTransferId id) => id.Value;
}
