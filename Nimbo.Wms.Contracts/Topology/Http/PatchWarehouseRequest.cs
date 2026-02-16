namespace Nimbo.Wms.Contracts.Topology.Http;

public sealed record PatchWarehouseRequest(
    string? Code = null,
    string? Name = null,
    string? Address = null,
    string? Description = null
);
