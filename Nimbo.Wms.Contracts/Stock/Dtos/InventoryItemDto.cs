using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Contracts.Stock.Dtos;

public sealed record InventoryItemDto(
    Guid Id,
    Guid ItemId,
    Guid WarehouseId,
    Guid LocationId,
    QuantityDto Quantity,
    string Status,
    Guid? BatchId,
    string? SerialNumber,
    decimal? UnitCost
);
