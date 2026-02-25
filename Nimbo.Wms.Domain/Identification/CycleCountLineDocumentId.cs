using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct CycleCountLineDocumentId : IEntityId
{
    public CycleCountLineDocumentId(Guid value)
    {
        EntityIdExtensions.EnsureNotEmpty<CycleCountLineDocumentId>(value);
        Value = value;
    }
    
    public Guid Value { get; }

    public static CycleCountLineDocumentId New() => EntityIdExtensions.New(id => new CycleCountLineDocumentId(id));
    
    public static CycleCountLineDocumentId From(Guid guid) => EntityIdExtensions.From(guid, id => new CycleCountLineDocumentId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(CycleCountLineDocumentId id) => id.Value;
    
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is CycleCountLineDocumentId id && Value.Equals(id.Value);

    public bool Equals(CycleCountLineDocumentId other) => Value.Equals(other.Value);
    
    public override int GetHashCode() => Value.GetHashCode();
    
    public static bool operator ==(CycleCountLineDocumentId left, CycleCountLineDocumentId right) => left.Value == right.Value;
    
    public static bool operator !=(CycleCountLineDocumentId left, CycleCountLineDocumentId right) => left.Value != right.Value;
}
