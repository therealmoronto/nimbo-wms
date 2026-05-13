using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Topology;

[PublicAPI]
public sealed record AddZoneRequest(
    Guid WarehouseGuid,
    string Code,
    string Name,
    string Type
);

[PublicAPI]
public sealed record AddZoneResponse(Guid ZoneId);
