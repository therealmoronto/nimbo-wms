namespace Nimbo.Wms.Contracts.Common.Dtos;

public sealed record QuantityDto
{
    public decimal Value { get; set; }

    public required string Uom { get; set; }

    public bool IsZero { get; set; }
}
