using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Topology;

[PublicAPI]
public sealed record CreateWarehouseRequest(
    string Code,
    string Name
);

[PublicAPI]
public sealed record CreateWarehouseResponse(Guid Id);
