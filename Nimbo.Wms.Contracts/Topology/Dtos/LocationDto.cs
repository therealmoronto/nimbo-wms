using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Contracts.Topology.Dtos;

public sealed record LocationDto(
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
