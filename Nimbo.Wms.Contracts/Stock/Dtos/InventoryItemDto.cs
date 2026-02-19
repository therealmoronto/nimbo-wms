using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Contracts.Stock.Dtos;

public sealed record InventoryItemDto(
    Guid Id,
    Guid ItemId,
    Guid WarehouseId,
    Guid LocationId,
    Quantity Quantity,
    InventoryStatus Status,
    Guid? BatchId,
    string? SerialNumber,
    decimal? UnitCost
);
