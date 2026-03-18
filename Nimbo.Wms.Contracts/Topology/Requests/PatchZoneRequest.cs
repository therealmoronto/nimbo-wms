using MediatR;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;

public sealed record PatchZoneRequest(
    Guid ZoneGuid,

    string? Code = null,
    string? Name = null,
    ZoneType? Type = null,

    decimal? MaxWeightKg = null,
    decimal? MaxVolumeM3 = null,

    bool? IsQuarantine = null,
    bool? IsDamagedArea = null
) : IRequest;
