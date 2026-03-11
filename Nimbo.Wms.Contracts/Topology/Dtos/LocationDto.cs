using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Contracts.Topology.Dtos;

public sealed class LocationDto
{
    public Guid WarehouseId { get; set; }

    public Guid ZoneId { get; set; }

    public Guid Id { get; set; }

    public string Code { get; set; }

    public string Type { get; set; }

    public decimal? MaxWeightKg { get; set; }

    public decimal? MaxVolumeM3 { get; set; }

    public bool IsSingleItemOnly { get; set; }

    public bool IsPickingLocation { get; set; }

    public bool IsReceivingLocation { get; set; }

    public bool IsShippingLocation { get; set; }

    public bool IsActive { get; set; }

    public bool IsBlocked { get; set; }

    public string? Aisle { get; set; }

    public string? Rack { get; set; }

    public string? Level { get; set; }

    public string? Position { get; set; }
}
