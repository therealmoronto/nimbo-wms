using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;

public interface IBatchRepository : IEntityRepository<Batch, BatchId>
{
    Task<Batch> FindOrCreateAsync(ItemId itemId, string batchNumber, DateTime? expiryDate, CancellationToken ct = default);
}
