namespace Nimbo.Wms.Contracts.Topology.Http;

public sealed record CreateWarehouseRequest(
    string Code,
    string Name
);
