using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Contracts.Topology.Dtos;

public sealed record LocationDto(
    WarehouseId WarehouseId,
    ZoneId ZoneId,
    LocationId Id,
    string Code,
    LocationType Type,
    decimal? MaxWeightKg,
    decimal? MaxVolumeM3,
    bool IsSingleItemOnly,
    bool IsPickingLocation,
    bool IsReceivingLocation,
    bool IsShippingLocation,
    bool IsActive,
    bool IsBlocked,
    string? Aisle,
    string? Rack,
    string? Level,
    string? Position
);
