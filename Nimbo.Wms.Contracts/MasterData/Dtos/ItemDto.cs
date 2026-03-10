using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Contracts.MasterData.Dtos;

public sealed class ItemDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string InternalSku { get; set; }

    public string Barcode { get; set; }

    public string BaseUomCode { get; set; }

    public string? Manufacturer { get; set; }

    public decimal? WeightKg { get; set; }

    public decimal? VolumeM3 { get; set; }
}
