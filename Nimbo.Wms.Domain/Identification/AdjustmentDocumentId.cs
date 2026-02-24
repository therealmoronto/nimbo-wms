using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct AdjustmentDocumentId : IEntityId
{
    public AdjustmentDocumentId(Guid value)
    {
        EntityIdExtensions.EnsureNotEmpty<AdjustmentDocumentId>(value);
        Value = value;
    }

    public Guid Value { get; }

    public static AdjustmentDocumentId New() => EntityIdExtensions.New(id => new AdjustmentDocumentId(id));

    public static AdjustmentDocumentId From(Guid guid) => EntityIdExtensions.From(guid, id => new AdjustmentDocumentId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(AdjustmentDocumentId id) => id.Value;
    
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is AdjustmentDocumentId id && Value.Equals(id.Value);

    public bool Equals(AdjustmentDocumentId other) => Value.Equals(other.Value);
    
    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(AdjustmentDocumentId left, AdjustmentDocumentId right) => left.Value == right.Value;

    public static bool operator !=(AdjustmentDocumentId left, AdjustmentDocumentId right) => left.Value != right.Value;
}
