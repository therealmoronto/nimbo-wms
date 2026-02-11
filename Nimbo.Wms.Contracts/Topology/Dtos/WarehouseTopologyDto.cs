using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Contracts.Topology.Dtos;

public sealed record WarehouseTopologyDto(
    Guid Id,
    string Code,
    string Name,
    IReadOnlyList<ZoneDto> Zones,
    IReadOnlyList<LocationDto> Locations
);
