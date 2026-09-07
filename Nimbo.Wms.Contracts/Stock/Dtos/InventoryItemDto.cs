using JetBrains.Annotations;
using Nimbo.Wms.Contracts.ValueObject;

namespace Nimbo.Wms.Contracts.Stock.Dtos;

[PublicAPI]
public sealed record InventoryItemDto(
    Guid Id,
    Guid ItemId,
    Guid WarehouseId,
    Guid LocationId,
    QuantityDto Quantity,
    string Status,
    Guid StockLotId,
    string? SerialNumber,
    decimal? UnitCost
);
