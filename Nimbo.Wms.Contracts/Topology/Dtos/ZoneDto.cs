using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Contracts.Topology.Dtos;

public sealed record ZoneDto(
    ZoneId Id,
    string Code,
    string Name
);
