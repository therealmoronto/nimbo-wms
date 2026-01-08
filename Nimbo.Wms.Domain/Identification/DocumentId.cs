using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct DocumentId : IEntityId
{
    public DocumentId(Guid value)
    {
        EntityId.EnsureNotEmpty<DocumentId>(value);
        Value = value;
    }
    
    public Guid Value { get; }

    public static DocumentId New() => EntityId.New(id => new DocumentId(id));
    
    public static DocumentId From(Guid guid) => EntityId.From(guid, id => new DocumentId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(DocumentId id) => id.Value;
}
