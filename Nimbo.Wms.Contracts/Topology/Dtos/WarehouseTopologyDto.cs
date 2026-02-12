namespace Nimbo.Wms.Contracts.Topology.Dtos;

public sealed record WarehouseTopologyDto(
    Guid Id,
    string Code,
    string Name,
    string? Address,
    string? Description,
    IReadOnlyList<ZoneDto> Zones,
    IReadOnlyList<LocationDto> Locations
);
