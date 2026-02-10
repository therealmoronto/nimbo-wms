using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Contracts.Topology.Dtos;

public sealed record ZoneDto(
    WarehouseId WarehouseId,
    ZoneId Id,
    string Code,
    string Name,
    ZoneType Type,
    decimal? MaxWeightKg,
    decimal? MaxVolumeM3,
    bool IsQuarantine,
    bool IsDamagedArea
);
