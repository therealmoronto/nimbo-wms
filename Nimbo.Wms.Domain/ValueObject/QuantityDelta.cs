using JetBrains.Annotations;
using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Domain.ValueObject;

[PublicAPI]
public sealed class QuantityDelta
{
    private QuantityDelta() { } // EF

    public QuantityDelta(decimal value, UnitOfMeasure uom)
    {
        if (value == 0m)
            throw new DomainException("Quantity delta cannot be zero.");

        Value = value;
        Uom = uom;
    }

    public decimal Value { get; private set; }

    public UnitOfMeasure Uom { get; private set; }

    public static QuantityDelta Increase(decimal value, UnitOfMeasure uom)
    {
        return new QuantityDelta(Math.Abs(value), uom);
    }

    public static QuantityDelta Decrease(decimal value, UnitOfMeasure uom)
    {
        return new QuantityDelta(-Math.Abs(value), uom);
    }

    public Quantity GetAbsQuantity() => new(Math.Abs(Value), Uom);
}
