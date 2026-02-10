using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Contracts.Topology.Dtos;

public sealed record LocationDto(
    LocationId Id,
    ZoneId ZoneId,
    string Code
);
