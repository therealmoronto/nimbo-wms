using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Domain.Common;
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
        StockLotId? stockLotId,
        CancellationToken ct = default)
    {
        var query = DbContext.Set<InventoryItem>()
            .Where(i => i.WarehouseId == warehouseId && i.LocationId == locationId && i.ItemId == itemId);

        if (stockLotId is not null)
            return await query.FirstOrDefaultAsync(i => i.StockLotId == stockLotId.Value, ct);

        var matches = await query.Take(2).ToListAsync(ct);
        if (matches.Count > 1)
            throw new DomainException(
                $"Multiple stock lots exist for Item {itemId} at Location {locationId}; StockLotId must be specified.");

        return matches.SingleOrDefault();
    }
}
