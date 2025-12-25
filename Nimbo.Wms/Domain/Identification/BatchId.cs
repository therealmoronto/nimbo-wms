namespace Nimbo.Wms.Domain.Identification;

public readonly struct BatchId : IEntityId
{
    public BatchId(Guid value)
    {
        EntityId.EnsureNotEmpty<BatchId>(value);
        Value = value;
    }
    
    public Guid Value { get; }
    
    public static BatchId New() => EntityId.New(id => new BatchId(id));
    
    public static BatchId From(Guid guid) => EntityId.From(guid, id => new BatchId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(BatchId id) => id.Value;
}
