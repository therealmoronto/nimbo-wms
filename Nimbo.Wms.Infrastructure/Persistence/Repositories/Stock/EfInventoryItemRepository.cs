using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.Stock;

internal sealed class EfInventoryItemRepository : EfEntityRepository<InventoryItem, InventoryItemId>, IInventoryItemRepository
{
    public EfInventoryItemRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }

    public async Task<InventoryItem?> GetByCriteriaAsync(
        WarehouseId warehouseId,
        LocationId locationId,
        ItemId itemId,
        CancellationToken ct = default)
    {
        return await DbContext.Set<InventoryItem>()
            .Where(i => i.WarehouseId == warehouseId && i.LocationId == locationId && i.ItemId == itemId)
            .FirstOrDefaultAsync(ct);
    }
}
