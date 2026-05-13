using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Topology;

[PublicAPI]
public sealed record PatchLocationRequest(
    Guid LocationGuid,

    string? Code = null,
    string? Type = null,

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
