namespace Nimbo.Wms.Contracts.Common.Dtos;

public sealed class QuantityDto
{
    public decimal Value { get; set; }

    public required string Uom { get; set; }
}
