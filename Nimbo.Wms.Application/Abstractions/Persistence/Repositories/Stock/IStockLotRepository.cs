using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;

public interface IStockLotRepository : IEntityRepository<StockLot, StockLotId>
{
    // No custom members: a StockLot is either always-created (Receiving posting) or looked up by
    // primary key (validating an externally-supplied StockLotId on the other document types) — both
    // covered by the base IEntityRepository. Reads for "available lots to pick from" go through the
    // CQRS query side (GetAvailableStockLotsQuery), not this repository.
}
