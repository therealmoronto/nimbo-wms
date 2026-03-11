using JetBrains.Annotations;

namespace Nimbo.Wms.Contracts.Topology.Dtos;

[PublicAPI]
public sealed class LocationDto(
    Guid WarehouseId,
    Guid ZoneId,
    Guid Id,
    string Code,
    string Type,
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
