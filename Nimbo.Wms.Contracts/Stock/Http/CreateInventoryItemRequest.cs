using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Contracts.Stock.Http;

public sealed record CreateInventoryItemRequest(
    Guid ItemId,
    Guid WarehouseId,
    Guid LocationId,
    Quantity Quantity,
    InventoryStatus Status,
    Guid? BatchId,
    string? SerialNumber,
    decimal? UnitCost
);
