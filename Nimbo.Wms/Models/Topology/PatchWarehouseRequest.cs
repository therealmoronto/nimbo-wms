using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Topology;

[PublicAPI]
public sealed record PatchWarehouseRequest(
    Guid WarehouseGuid,
    string? Code = null,
    string? Name = null,
    string? Address = null,
    string? Description = null
);
