namespace Nimbo.Wms.Contracts.Stock.Http;

public sealed record GetBatchesRequest(Guid? ItemId, Guid? SupplierId);
