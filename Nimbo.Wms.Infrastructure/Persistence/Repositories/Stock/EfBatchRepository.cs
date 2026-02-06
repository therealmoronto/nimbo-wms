using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.Stock;

internal sealed class EfBatchRepository : EfRepository<Batch, BatchId>, IBatchRepository
{
    public EfBatchRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }
}
