using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct ReceivingDocumentId : IEntityId
{
    public ReceivingDocumentId(Guid value)
    {
        EntityIdExtensions.EnsureNotEmpty<ReceivingDocumentId>(value);
        Value = value;
    }

    public Guid Value { get; }

    public static ReceivingDocumentId New() => EntityIdExtensions.New(id => new ReceivingDocumentId(id));

    public static ReceivingDocumentId From(Guid guid) => EntityIdExtensions.From(guid, id => new ReceivingDocumentId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(ReceivingDocumentId id) => id.Value;
    
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is ReceivingDocumentId id && Value.Equals(id.Value);

    public bool Equals(ReceivingDocumentId other) => Value.Equals(other.Value);
    
    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(ReceivingDocumentId left, ReceivingDocumentId right) => left.Value == right.Value;

    public static bool operator !=(ReceivingDocumentId left, ReceivingDocumentId right) => left.Value != right.Value;
}
