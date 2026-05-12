using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Topology;

[PublicAPI]
public sealed record PatchZoneRequest(
    Guid ZoneGuid,

    string? Code = null,
    string? Name = null,
    string? Type = null,

    decimal? MaxWeightKg = null,
    decimal? MaxVolumeM3 = null,

    bool? IsQuarantine = null,
    bool? IsDamagedArea = null
);
