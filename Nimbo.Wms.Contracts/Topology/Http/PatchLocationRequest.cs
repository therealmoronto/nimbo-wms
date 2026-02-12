using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Contracts.Topology.Http;

public sealed record PatchLocationRequest(
    string? Code = null,
    LocationType? Type = null,

    decimal? MaxWeightKg = null,
    decimal? MaxVolumeM3 = null,

    bool? IsSingleItemOnly = null,
    bool? IsPickingLocation = null,
    bool? IsReceivingLocation = null,
    bool? IsShippingLocation = null,

    bool? IsActive = null,
    bool? IsBlocked = null,

    string? Aisle = null,
    string? Rack = null,
    string? Level = null,
    string? Position = null
);
