using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Ledger;
using Nimbo.Wms.Domain.Entities.Ledger;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.Ledger;

[PublicAPI]
internal sealed class EfStockLedgerEntryRepository : EfEntityRepository<StockLedgerEntry, StockLedgerEntryId>, IStockLedgerEntryRepository
{
    public EfStockLedgerEntryRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }

    public Task<List<StockLedgerEntry>> GetByInventoryItemIdAsync(InventoryItemId inventoryItemId, CancellationToken ct = default)
    {
        return Set.Where(e => e.InventoryItemId == inventoryItemId)
            .ToListAsync(ct);
    }
}
