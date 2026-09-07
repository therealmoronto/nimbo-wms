namespace Nimbo.Wms.Contracts.ValueObject;

public sealed class QuantityDeltaDto
{
    public decimal Value { get; set; }

    public required string Uom { get; set; }

    public bool IsZero { get; set; }
}
