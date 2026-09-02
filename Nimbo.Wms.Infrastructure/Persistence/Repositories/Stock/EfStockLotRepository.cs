using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.Stock;

internal sealed class EfStockLotRepository : EfEntityRepository<StockLot, StockLotId>, IStockLotRepository
{
    public EfStockLotRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }
}
