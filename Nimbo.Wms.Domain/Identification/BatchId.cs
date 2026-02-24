using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct BatchId : IEntityId
{
    public BatchId(Guid value)
    {
        EntityIdExtensions.EnsureNotEmpty<BatchId>(value);
        Value = value;
    }
    
    public Guid Value { get; }

    public static BatchId New() => EntityIdExtensions.New(id => new BatchId(id));

    public static BatchId From(Guid guid) => EntityIdExtensions.From(guid, id => new BatchId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(BatchId id) => id.Value;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is BatchId id && Value.Equals(id.Value);

    public bool Equals(BatchId other) => Value.Equals(other.Value);
    
    public override int GetHashCode() => Value.GetHashCode();
    
    public static bool operator ==(BatchId left, BatchId right) => left.Value == right.Value;
    
    public static bool operator !=(BatchId left, BatchId right) => left.Value != right.Value;
}
