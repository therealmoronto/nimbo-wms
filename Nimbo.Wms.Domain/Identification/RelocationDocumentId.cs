using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public struct RelocationDocumentId : IEntityId
{
    public RelocationDocumentId(Guid value)
    {
        EntityIdExtensions.EnsureNotEmpty<RelocationDocumentId>(value);
        Value = value;
    }

    public Guid Value { get; }

    public static RelocationDocumentId New() => EntityIdExtensions.New(id => new RelocationDocumentId(id));

    public static RelocationDocumentId From(Guid guid) => EntityIdExtensions.From(guid, id => new RelocationDocumentId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(RelocationDocumentId id) => id.Value;
    
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is RelocationDocumentId id && Value.Equals(id.Value);

    public bool Equals(RelocationDocumentId other) => Value.Equals(other.Value);
    
    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(RelocationDocumentId left, RelocationDocumentId right) => left.Value == right.Value;

    public static bool operator !=(RelocationDocumentId left, RelocationDocumentId right) => left.Value != right.Value;
}
