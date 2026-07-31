using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.Stock;

internal sealed class EfBatchRepository : EfEntityRepository<Batch, BatchId>, IBatchRepository
{
    public EfBatchRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }

    public async Task<Batch> FindOrCreateAsync(ItemId itemId, string batchNumber, DateTime? expiryDate, CancellationToken ct = default)
    {
        var batch = await Set.FirstOrDefaultAsync(b => b.ItemId == itemId && b.BatchNumber == batchNumber, ct);
        if (batch is not null)
            return batch;

        batch = new Batch(BatchId.New(), itemId, batchNumber, expiryDate: expiryDate);
        await AddAsync(batch, ct);
        return batch;
    }
}
