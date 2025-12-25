using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.References.Extensions;

public static class QuantityExtensions
{
    public static Quantity Piece(this int value) => new(value, UnitOfMeasure.Piece);
    public static Quantity Piece(this decimal value) => new(value, UnitOfMeasure.Piece);

    public static Quantity Kilogram(this int value) => new(value, UnitOfMeasure.Kilogram);
    public static Quantity Kilogram(this decimal value) => new(value, UnitOfMeasure.Kilogram);

    public static Quantity Gram(this int value) => new(value, UnitOfMeasure.Gram);
    public static Quantity Gram(this decimal value) => new(value, UnitOfMeasure.Gram);

    public static Quantity Liter(this int value) => new(value, UnitOfMeasure.Liter);
    public static Quantity Liter(this decimal value) => new(value, UnitOfMeasure.Liter);

    public static Quantity Mililiter(this int value) => new(value, UnitOfMeasure.Mililiter);
    public static Quantity Mililiter(this decimal value) => new(value, UnitOfMeasure.Mililiter);
}
