using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;

public interface IInventoryItemRepository : IEntityRepository<InventoryItem, InventoryItemId>
{
    /// <summary>
    /// Looks up an InventoryItem by (WarehouseId, LocationId, ItemId), optionally narrowed to a specific
    /// StockLotId. When stockLotId is null and more than one stock lot exists at that item/location, the
    /// lookup is ambiguous and throws a DomainException — callers must pass an explicit StockLotId once
    /// more than one lot can coexist there.
    /// </summary>
    Task<InventoryItem?> GetByCriteriaAsync(WarehouseId warehouseId, LocationId locationId, ItemId itemId, StockLotId? stockLotId, CancellationToken ct = default);
}
