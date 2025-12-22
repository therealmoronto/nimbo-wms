using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct SupplierId : IEntityId
{
    public SupplierId(Guid value)
    {
        EntityId.EnsureNotEmpty<SupplierId>(value);
        Value = value;
    }
    
    public Guid Value { get; }

    public static SupplierId New() => EntityId.New(id => new SupplierId(id));
    
    public static SupplierId From(Guid guid) => EntityId.From(guid, id => new SupplierId(id));
}
