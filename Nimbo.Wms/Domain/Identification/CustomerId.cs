namespace Nimbo.Wms.Domain.Identification;

public readonly struct CustomerId : IEntityId
{
    public CustomerId(Guid value)
    {
        EntityId.EnsureNotEmpty<CustomerId>(value);
        Value = value;
    }
    
    public Guid Value { get; }
    
    public static CustomerId New() => EntityId.New(id => new CustomerId(id));
    
    public static CustomerId From(Guid guid) => EntityId.From(guid, id => new CustomerId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(CustomerId id) => id.Value;
}
