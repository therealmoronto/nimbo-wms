using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Contracts.Topology.Http;

public sealed record AddLocationRequest(
    Guid ZoneId,
    string Code,
    LocationType Type
);
