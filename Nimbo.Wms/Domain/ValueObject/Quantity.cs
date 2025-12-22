namespace Nimbo.Wms.Domain.References;

/// <summary>
/// Immutable quantity with a unit of measure.
/// </summary>
public readonly record struct Quantity(decimal Value, UnitOfMeasure Uom)
{
    public static Quantity Zero(UnitOfMeasure uom) => new(0m, uom);

    public bool IsZero => Value == 0m;

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
    
    public static Quantity operator +(Quantity a, Quantity b) => a.Add(b);
    public static Quantity operator -(Quantity a, Quantity b) => a.Subtract(b);

    private void EnsureSameUom(Quantity other)
    {
        if (Uom != other.Uom)
            throw new InvalidOperationException($"UoM mismatch: {Uom} vs {other.Uom}");
    }

    public override string ToString() => $"{Value} {Uom}";
}
