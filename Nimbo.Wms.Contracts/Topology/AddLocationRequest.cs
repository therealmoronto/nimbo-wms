using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Contracts.Topology;

public sealed record AddLocationRequest(
    Guid ZoneId,
    string Code,
    LocationType Type
);
