using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Ledger;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Entities.Documents.Relocation;
using Nimbo.Wms.Domain.Entities.Ledger;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Application.Services.Documents;

public class RelocationDocumentPostingService : IDocumentPostingService<RelocationDocument>
{
    private readonly IStockLedgerEntryRepository _stockLedgerEntryRepo;
    private readonly IInventoryItemRepository _inventoryItemRepo;

    public RelocationDocumentPostingService(
        IStockLedgerEntryRepository stockLedgerEntryRepo,
        IInventoryItemRepository inventoryItemRepo)
    {
        _stockLedgerEntryRepo = stockLedgerEntryRepo;
        _inventoryItemRepo = inventoryItemRepo;
    }

    public async Task PostAsync(RelocationDocument document, CancellationToken ct = default)
    {
        if (document.Status is not RelocationStatus.InProgress)
            throw new DomainException("Document must be in progress to post");

        foreach (var line in document.Lines)
        {
            var sourceItem = await _inventoryItemRepo.GetByCriteriaAsync(document.WarehouseId, line.From, line.ItemId, ct);
            if (sourceItem is null || sourceItem.Quantity.Value < line.Quantity.Value)
                throw new DomainException("Source item does not exist or insufficient quantity");

            sourceItem.Decrease(line.Quantity);

            var outLedger = new StockLedgerEntry(
                sourceItem.Id,
                sourceItem.ItemId,
                sourceItem.LocationId,
                sourceItem.WarehouseId,
                line.Quantity.ToDelta().Negate(),
                sourceItem.Quantity,
                document.Id,
                line.Id,
                LedgerTransactionType.TransferOut,
                DateTime.UtcNow);

            await _stockLedgerEntryRepo.AddAsync(outLedger, ct);

            var targetItem = await _inventoryItemRepo.GetByCriteriaAsync(
                document.WarehouseId,
                line.To,
                line.ItemId,
                ct);

            if (targetItem is null)
            {
                targetItem = new InventoryItem(
                    InventoryItemId.New(),
                    line.ItemId,
                    document.WarehouseId,
                    line.To,
                    Quantity.Zero(line.Quantity.Uom));

                await _inventoryItemRepo.AddAsync(targetItem, ct);
            }

            targetItem.Increase(line.Quantity);

            var inLedger = new StockLedgerEntry(
                targetItem.Id,
                targetItem.ItemId,
                targetItem.LocationId,
                targetItem.WarehouseId,
                line.Quantity.ToDelta(),
                targetItem.Quantity,
                document.Id,
                line.Id,
                LedgerTransactionType.TransferIn,
                DateTime.UtcNow);

            await _stockLedgerEntryRepo.AddAsync(inLedger, ct);
        }

        document.Post();
    }
}
