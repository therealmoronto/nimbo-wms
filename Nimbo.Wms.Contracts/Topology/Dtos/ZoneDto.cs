using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Contracts.Topology.Dtos;

public sealed class ZoneDto
{
    public Guid WarehouseId { get; set; }

    public Guid Id { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public string Type { get; set; }

    public decimal? MaxWeightKg { get; set; }

    public decimal? MaxVolumeM3 { get; set; }

    public bool IsQuarantine { get; set; }

    public bool IsDamagedArea { get; set; }
}
