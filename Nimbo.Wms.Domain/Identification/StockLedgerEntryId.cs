using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct StockLedgerEntryId : IEntityId
{
    public StockLedgerEntryId(Guid value)
    {
        EntityIdExtensions.EnsureNotEmpty<StockLedgerEntryId>(value);
        Value = value;
    }

    public Guid Value { get; }

    public static StockLedgerEntryId New() => EntityIdExtensions.New(id => new StockLedgerEntryId(id));

    public static StockLedgerEntryId From(Guid guid) => EntityIdExtensions.From(guid, id => new StockLedgerEntryId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(StockLedgerEntryId id) => id.Value;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is StockLedgerEntryId id && Value.Equals(id.Value);

    public bool Equals(StockLedgerEntryId other) => Value.Equals(other.Value);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(StockLedgerEntryId left, StockLedgerEntryId right) => left.Value == right.Value;

    public static bool operator !=(StockLedgerEntryId left, StockLedgerEntryId right) => left.Value != right.Value;
}
