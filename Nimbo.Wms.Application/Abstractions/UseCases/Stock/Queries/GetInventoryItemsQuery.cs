using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Stock.Queries;

public sealed record GetInventoryItemsQuery(WarehouseId? WarehouseId, ItemId? ItemId, BatchId? BatchId);
