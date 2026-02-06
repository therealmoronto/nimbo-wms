using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Domain.Documents.Transfer;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.Documents;

internal sealed class EfTransferOrderRepository : EfRepository<TransferOrder, TransferOrderId>, ITransferOrderRepository
{
    public EfTransferOrderRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }
    
    public override Task<TransferOrder?> GetByIdAsync(TransferOrderId id, CancellationToken ct = default)
    {
        return Set.Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id.Equals(id), ct);
    }
}
