using JetBrains.Annotations;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Ledger;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Entities.Documents.Receiving;
using Nimbo.Wms.Domain.Entities.Ledger;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Application.Services.Documents;

[PublicAPI]
public class ReceivingDocumentPostingService : IDocumentPostingService<ReceivingDocument>
{
    private readonly IStockLedgerEntryRepository _stockLedgerEntryRepo;
    private readonly IInventoryItemRepository _inventoryItemRepo;

    public ReceivingDocumentPostingService(IStockLedgerEntryRepository stockLedgerEntryRepo, IInventoryItemRepository inventoryItemRepo)
    {
        _stockLedgerEntryRepo = stockLedgerEntryRepo;
        _inventoryItemRepo = inventoryItemRepo;
    }

    public async Task PostAsync(ReceivingDocument document, CancellationToken ct = default)
    {
        if (document.Status is not ReceivingStatus.InProgress)
            throw new DomainException("Document must be in progress to post");

        foreach (var line in document.Lines)
        {
            if (line.ReceivedQuantity.IsZero)
                continue;

            var inventoryItem = await _inventoryItemRepo.GetByCriteriaAsync(document.WarehouseId, line.ToLocationId, line.ItemId, ct);
            if (inventoryItem is null)
            {
                var inventoryItemId = InventoryItemId.New();
                inventoryItem = new InventoryItem(
                    inventoryItemId,
                    line.ItemId,
                    document.WarehouseId,
                    line.ToLocationId,
                    Quantity.Zero(line.ReceivedQuantity.Uom));

                await _inventoryItemRepo.AddAsync(inventoryItem, ct);
            }

            inventoryItem.Increase(line.ReceivedQuantity);

            var ledgerEntity = new StockLedgerEntry(
                inventoryItem.Id,
                inventoryItem.ItemId,
                inventoryItem.LocationId,
                inventoryItem.WarehouseId,
                line.ReceivedQuantity.ToDelta(),
                inventoryItem.Quantity,
                document.Id,
                line.Id,
                LedgerTransactionType.Receipt,
                DateTime.UtcNow);

            await _stockLedgerEntryRepo.AddAsync(ledgerEntity, ct);
        }

        document.Post();
    }
}
