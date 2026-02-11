using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Contracts.Topology;

public sealed record AddZoneRequest(
    string Code,
    string Name,
    ZoneType Type
);
