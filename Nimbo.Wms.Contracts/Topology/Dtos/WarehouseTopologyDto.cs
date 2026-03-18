using JetBrains.Annotations;

namespace Nimbo.Wms.Contracts.Topology.Dtos;

[PublicAPI]
public sealed record WarehouseTopologyDto(
    Guid Id,
    string Code,
    string Name,
    string? Address,
    string? Description,
    bool IsActive,
    IReadOnlyList<ZoneDto> Zones,
    IReadOnlyList<LocationDto> Locations
);
