using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Domain.Documents.Audit;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.Documents;

internal sealed class EfInventoryCountRepository : EfRepository<InventoryCount, InventoryCountId>, IInventoryCountRepository
{
    public EfInventoryCountRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }

    public override Task<InventoryCount?> GetByIdAsync(InventoryCountId id, CancellationToken ct = default)
    {
        return Set.Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id.Equals(id), ct);
    }
}
