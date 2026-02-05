using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct DocumentId : IEntityId
{
    public DocumentId(Guid value)
    {
        EntityIdExtensions.EnsureNotEmpty<DocumentId>(value);
        Value = value;
    }
    
    public Guid Value { get; }

    public static DocumentId New() => EntityIdExtensions.New(id => new DocumentId(id));
    
    public static DocumentId From(Guid guid) => EntityIdExtensions.From(guid, id => new DocumentId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(DocumentId id) => id.Value;
    
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is DocumentId id && Value.Equals(id.Value);

    public bool Equals(DocumentId other) => Value.Equals(other.Value);
    
    public override int GetHashCode() => Value.GetHashCode();
}
