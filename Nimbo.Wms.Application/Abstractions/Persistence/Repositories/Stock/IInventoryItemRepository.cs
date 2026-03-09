using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;

public interface IInventoryItemRepository : IEntityRepository<InventoryItem, InventoryItemId>
{
    Task<InventoryItem?> GetByCriteriaAsync(WarehouseId warehouseId, LocationId locationId, ItemId itemId, CancellationToken ct = default);
}
