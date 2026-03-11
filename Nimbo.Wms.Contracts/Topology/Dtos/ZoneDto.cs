using JetBrains.Annotations;

namespace Nimbo.Wms.Contracts.Topology.Dtos;

[PublicAPI]
public sealed record ZoneDto(
    Guid WarehouseId,
    Guid Id,
    string Code,
    string Name,
    string Type,
    decimal? MaxWeightKg,
    decimal? MaxVolumeM3,
    bool IsQuarantine,
    bool IsDamagedArea
);
