using JetBrains.Annotations;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Domain.ValueObject;

[PublicAPI]
public readonly record struct QuantityDelta
{
    public QuantityDelta(decimal value, UnitOfMeasure uom)
    {
        Value = value;
        Uom = uom;
    }

    public bool IsZero => Value == 0m;

    public decimal Value { get; }

    public UnitOfMeasure Uom { get; }

    public QuantityDelta Add(QuantityDelta other)
    {
        EnsureSameUom(other);
        return new QuantityDelta(Value + other.Value, Uom);
    }

    public QuantityDelta Subtract(QuantityDelta other)
    {
        EnsureSameUom(other);
        var result = Value - other.Value;
        if (result < 0m)
            throw new InvalidOperationException("Resulting quantity cannot be negative.");

        return new QuantityDelta(result, Uom);
    }

    public static QuantityDelta operator +(QuantityDelta a, QuantityDelta b) => a.Add(b);

    public static QuantityDelta operator -(QuantityDelta a, QuantityDelta b) => a.Subtract(b);

    public static bool operator <(QuantityDelta left, QuantityDelta right) => left.Value < right.Value;

    public static bool operator >(QuantityDelta left, QuantityDelta right) => left.Value > right.Value;

    public static bool operator <(QuantityDelta left, decimal right) => left.Value < right;

    public static bool operator >(QuantityDelta left, decimal right) => left.Value > right;

    public static bool operator <=(QuantityDelta left, QuantityDelta right) => left.Value <= right.Value;

    public static bool operator >=(QuantityDelta left, QuantityDelta right) => left.Value >= right.Value;

    public static bool operator <=(QuantityDelta left, decimal right) => left.Value <= right;

    public static bool operator >=(QuantityDelta left, decimal right) => left.Value >= right;

    public static bool operator ==(QuantityDelta left, decimal right) => left.Value == right;

    public static bool operator !=(QuantityDelta left, decimal right) => left.Value != right;

    public QuantityDelta Increase(decimal value, UnitOfMeasure uom)
    {
        return new QuantityDelta(Math.Abs(value), uom);
    }

    public QuantityDelta Decrease(decimal value, UnitOfMeasure uom)
    {
        return new QuantityDelta(-Math.Abs(value), uom);
    }

    public QuantityDelta Negate() => new(-1m * Value, Uom);

    public Quantity GetAbsQuantity() => new(Math.Abs(Value), Uom);

    private void EnsureSameUom(QuantityDelta other)
    {
        if (Uom != other.Uom)
            throw new InvalidOperationException($"UoM mismatch: {Uom} vs {other.Uom}");
    }
}
