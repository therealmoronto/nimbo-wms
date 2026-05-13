using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Stock;

[PublicAPI]
public sealed record CreateInventoryItemRequest(
    Guid ItemId,
    Guid WarehouseId,
    Guid LocationId,
    decimal Quantity,
    string QuantityUom,
    string Status,
    Guid? BatchId,
    string? SerialNumber,
    decimal? UnitCost
);

[PublicAPI]
public sealed record CreateInventoryItemResponse(Guid Id);
