using JetBrains.Annotations;
using Nimbo.Wms.Domain.Entities.Ledger;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Ledger;

[PublicAPI]
public interface IStockLedgerEntryRepository : IEntityRepository<StockLedgerEntry, StockLedgerEntryId>
{
    Task<List<StockLedgerEntry>> GetByInventoryItemIdAsync(InventoryItemId inventoryItemId, CancellationToken ct = default);
}
