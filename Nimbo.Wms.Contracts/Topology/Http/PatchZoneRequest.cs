using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Contracts.Topology.Http;

public sealed record PatchZoneRequest(
    string? Code = null,
    string? Name = null,
    ZoneType? Type = null,

    decimal? MaxWeightKg = null,
    decimal? MaxVolumeM3 = null,

    bool? IsQuarantine = null,
    bool? IsDamagedArea = null
);
