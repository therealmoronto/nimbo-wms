using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.MasterData;

internal sealed class EfSupplierRepository : EfRepository<Supplier, SupplierId>, ISupplierRepository
{
    public EfSupplierRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }

    public async Task<Supplier?> GetByIdWithItemsAsync(SupplierId id, CancellationToken ct = default)
    {
        return await Set.Include(s => s.Items)
            .SingleOrDefaultAsync(s => s.Id == id, ct);
    }
}
