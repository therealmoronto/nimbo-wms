namespace Nimbo.Wms.Contracts.Stock.Http;

public sealed record GetInventoryItemRequest(Guid? WarehouseId, Guid? ItemId, Guid? BatchId);
