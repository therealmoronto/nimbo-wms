using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Identification;

[PublicAPI]
public readonly struct StockLotId : IEntityId
{
    public StockLotId(Guid value)
    {
        EntityIdExtensions.EnsureNotEmpty<StockLotId>(value);
        Value = value;
    }

    public Guid Value { get; }

    public static StockLotId New() => EntityIdExtensions.New(id => new StockLotId(id));

    public static StockLotId From(Guid guid) => EntityIdExtensions.From(guid, id => new StockLotId(id));

    public override string ToString() => Value.ToString("D");

    public static implicit operator Guid(StockLotId id) => id.Value;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is StockLotId id && Value.Equals(id.Value);

    public bool Equals(StockLotId other) => Value.Equals(other.Value);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(StockLotId left, StockLotId right) => left.Value == right.Value;

    public static bool operator !=(StockLotId left, StockLotId right) => left.Value != right.Value;
}
