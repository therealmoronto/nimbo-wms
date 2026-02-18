using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Contracts.MasterData.Http;

public sealed record AddSupplierItemRequest(
    SupplierId SupplierId,
    ItemId ItemId
);
