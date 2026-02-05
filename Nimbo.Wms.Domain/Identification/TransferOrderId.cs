using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct TransferOrderId : IEntityId, IEquatable<TransferOrderId>
{
    public TransferOrderId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("TransferOrderId cannot be empty.", nameof(value));

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
}
