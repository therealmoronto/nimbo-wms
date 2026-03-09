using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Domain.ValueObject;

/// <summary>
/// Immutable quantity with a unit of measure.
/// </summary>
public readonly record struct Quantity
{
    public Quantity(decimal value, UnitOfMeasure uom)
    {
        if (value < 0m)
            throw new ArgumentOutOfRangeException(nameof(value), "Value cannot be negative. If you want to represent a negative quantity, use QuantityDelta instead.");

        Value = value;
        Uom = uom;
    }

    public bool IsZero => Value == 0m;

    public decimal Value { get; }

    public UnitOfMeasure Uom { get; }

    public static Quantity Zero(UnitOfMeasure uom) => new(0m, uom);

    public Quantity Add(Quantity other)
    {
        EnsureSameUom(other);
        return new Quantity(Value + other.Value, Uom);
    }

    public Quantity Subtract(Quantity other)
    {
        EnsureSameUom(other);
        var result = Value - other.Value;
        if (result < 0m)
            throw new InvalidOperationException("Resulting quantity cannot be negative.");

        return new Quantity(result, Uom);
    }

    public Quantity ApplyDelta(QuantityDelta delta)
    {
        EnsureSameUom(delta);
        var value = Value + delta.Value;
        return new Quantity(value, Uom);
    }

    public QuantityDelta GetDelta(Quantity other)
    {
        EnsureSameUom(other);
        var value = Value - other.Value;
        return new QuantityDelta(value, Uom);
    }

    public QuantityDelta ToDelta() => new(Value, Uom);

    public static Quantity operator +(Quantity a, Quantity b) => a.Add(b);

    public static Quantity operator -(Quantity a, Quantity b) => a.Subtract(b);

    public static bool operator <(Quantity left, Quantity right) => left.Value < right.Value;

    public static bool operator >(Quantity left, Quantity right) => left.Value > right.Value;

    public static bool operator <(Quantity left, decimal right) => left.Value < right;

    public static bool operator >(Quantity left, decimal right) => left.Value > right;

    public static bool operator <=(Quantity left, Quantity right) => left.Value <= right.Value;

    public static bool operator >=(Quantity left, Quantity right) => left.Value >= right.Value;

    public static bool operator <=(Quantity left, decimal right) => left.Value <= right;

    public static bool operator >=(Quantity left, decimal right) => left.Value >= right;

    public static bool operator ==(Quantity left, decimal right) => left.Value == right;

    public static bool operator !=(Quantity left, decimal right) => left.Value != right;

    private void EnsureSameUom(Quantity other)
    {
        if (Uom != other.Uom)
            throw new InvalidOperationException($"UoM mismatch: {Uom} vs {other.Uom}");
    }

    private void EnsureSameUom(QuantityDelta delta)
    {
        if (Uom != delta.Uom)
            throw new InvalidOperationException($"UoM mismatch: {Uom} vs {delta.Uom}");
    }

    public override string ToString() => $"{Value} {Uom}";

    public static implicit operator decimal(Quantity quantity) => quantity.Value;
}
