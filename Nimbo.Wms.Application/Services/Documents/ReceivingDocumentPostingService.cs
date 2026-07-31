using JetBrains.Annotations;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Ledger;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Entities.Documents.Receiving;
using Nimbo.Wms.Domain.Entities.Ledger;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Application.Services.Documents;

[PublicAPI]
public sealed class ReceivingDocumentPostingService : IDocumentPostingService<ReceivingDocument>
{
    private readonly IStockLedgerEntryRepository _stockLedgerEntryRepo;
    private readonly IItemRepository _itemRepo;
    private readonly IInventoryItemRepository _inventoryItemRepo;
    private readonly IBatchRepository _batchRepo;

    public ReceivingDocumentPostingService(
        IStockLedgerEntryRepository stockLedgerEntryRepo,
        IItemRepository itemRepo,
        IInventoryItemRepository inventoryItemRepo,
        IBatchRepository batchRepo)
    {
        _stockLedgerEntryRepo = stockLedgerEntryRepo;
        _itemRepo = itemRepo;
        _inventoryItemRepo = inventoryItemRepo;
        _batchRepo = batchRepo;
    }

    public async Task PostAsync(ReceivingDocument document, CancellationToken ct = default)
    {
        if (document.Status is not ReceivingStatus.InProgress)
            throw new DomainException("Document must be in progress to post");

        foreach (var line in document.Lines)
        {
            if (line.ReceivedQuantity.IsZero)
                continue;

            var batch = !string.IsNullOrWhiteSpace(line.BatchNumber)
                ? await _batchRepo.FindOrCreateAsync(line.ItemId, line.BatchNumber, line.ExpiryDate, ct)
                : null;

            var item = await _itemRepo.GetByIdAsync(line.ItemId, ct);
            if (item is { IsBatchManaged: true } && batch is null)
                throw new DomainException($"Batch number is required for item {line.ItemId}");

            var inventoryItem = await _inventoryItemRepo.GetByCriteriaAsync(document.WarehouseId, line.ToLocationId, line.ItemId, batch?.Id, ct);
            if (inventoryItem is null)
            {
                var inventoryItemId = InventoryItemId.New();
                inventoryItem = new InventoryItem(
                    inventoryItemId,
                    line.ItemId,
                    document.WarehouseId,
                    line.ToLocationId,
                    Quantity.Zero(line.ReceivedQuantity.Uom),
                    batchId: batch?.Id);

                await _inventoryItemRepo.AddAsync(inventoryItem, ct);
            }

            inventoryItem.Increase(line.ReceivedQuantity);

            var ledgerEntity = new StockLedgerEntry(
                inventoryItem.Id,
                inventoryItem.ItemId,
                inventoryItem.BatchId,
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
