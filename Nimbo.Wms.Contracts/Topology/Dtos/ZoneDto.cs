using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Contracts.Topology.Dtos;

public sealed record ZoneDto(
    Guid WarehouseId,
    Guid Id,
    string Code,
    string Name,
    ZoneType Type,
    decimal? MaxWeightKg,
    decimal? MaxVolumeM3,
    bool IsQuarantine,
    bool IsDamagedArea
);
