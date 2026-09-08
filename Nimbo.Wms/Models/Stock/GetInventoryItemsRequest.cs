using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Stock.Dtos;

namespace Nimbo.Wms.Models.Stock;

[PublicAPI]
public sealed record GetInventoryItemsRequest(
    Guid? WarehouseId,
    Guid? ItemId,
    Guid? StockLotId
);

public sealed record GetInventoryItemsResponse(IReadOnlyList<InventoryItemDto> Value);
