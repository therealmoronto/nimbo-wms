using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Topology.Commands;

[PublicAPI]
public sealed record PatchZoneCommand(
    Guid ZoneGuid,

    string? Code = null,
    string? Name = null,
    string? Type = null,

    decimal? MaxWeightKg = null,
    decimal? MaxVolumeM3 = null,

    bool? IsQuarantine = null,
    bool? IsDamagedArea = null
) : IRequest, ITxRequest;
