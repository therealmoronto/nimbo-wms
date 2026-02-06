using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.Stock;

internal sealed class EfInventoryItemRepository : EfRepository<InventoryItem, InventoryItemId>, IInventoryItemRepository
{
    public EfInventoryItemRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }
}
