using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct TransferOrderId : IEntityId, IEquatable<TransferOrderId>
{
    public TransferOrderId(Guid value)
    {
        EntityIdExtensions.EnsureNotEmpty<TransferOrderId>(value);
        Value = value;
    }

    public Guid Value { get; }

    public static TransferOrderId New() => EntityIdExtensions.New(id => new TransferOrderId(id));

    public static TransferOrderId From(Guid guid) => EntityIdExtensions.From(guid, id => new TransferOrderId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(TransferOrderId id) => id.Value;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is TransferOrderId id && Value.Equals(id.Value);

    public bool Equals(TransferOrderId other) => Value.Equals(other.Value);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(TransferOrderId left, TransferOrderId right) => left.Value == right.Value;

    public static bool operator !=(TransferOrderId left, TransferOrderId right) => left.Value != right.Value;
}
