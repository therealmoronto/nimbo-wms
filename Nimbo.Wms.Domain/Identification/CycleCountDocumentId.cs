using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct CycleCountDocumentId : IEntityId
{
    public CycleCountDocumentId(Guid value)
    {
        EntityIdExtensions.EnsureNotEmpty<CycleCountDocumentId>(value);
        Value = value;
    }
    
    public Guid Value { get; }

    public static CycleCountDocumentId New() => EntityIdExtensions.New(id => new CycleCountDocumentId(id));
    
    public static CycleCountDocumentId From(Guid guid) => EntityIdExtensions.From(guid, id => new CycleCountDocumentId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(CycleCountDocumentId id) => id.Value;
    
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is CycleCountDocumentId id && Value.Equals(id.Value);

    public bool Equals(CycleCountDocumentId other) => Value.Equals(other.Value);
    
    public override int GetHashCode() => Value.GetHashCode();
    
    public static bool operator ==(CycleCountDocumentId left, CycleCountDocumentId right) => left.Value == right.Value;
    
    public static bool operator !=(CycleCountDocumentId left, CycleCountDocumentId right) => left.Value != right.Value;
}
