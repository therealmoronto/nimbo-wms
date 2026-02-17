namespace Nimbo.Wms.Contracts.Topology.Http;

public sealed record CreateSupplierRequest(
    string Code,
    string Name
);
