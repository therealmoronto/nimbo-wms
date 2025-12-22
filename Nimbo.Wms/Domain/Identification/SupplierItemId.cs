using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct SupplierItemId : IEntityId
{
    public SupplierItemId(Guid value)
    {
        EntityId.EnsureNotEmpty<SupplierItemId>(value);
        Value = value;
    }
    
    public Guid Value { get; }

    public static SupplierItemId New() => EntityId.New(id => new SupplierItemId(id));
    
    public static SupplierItemId From(Guid guid) => EntityId.From(guid, id => new SupplierItemId(id));
}
