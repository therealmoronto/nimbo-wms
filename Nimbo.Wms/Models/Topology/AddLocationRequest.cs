using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Topology;

[PublicAPI]
public sealed record AddLocationRequest(
    Guid WarehouseGuid,
    Guid ZoneGuid,
    string Code,
    string Type
);

[PublicAPI]
public sealed record AddLocationResponse(Guid LocationId);
