using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.MasterData;

internal sealed class EfItemRepository : EfEntityRepository<Item, ItemId>, IItemRepository
{
    public EfItemRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }
}
