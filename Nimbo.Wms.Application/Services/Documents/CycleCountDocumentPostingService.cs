using JetBrains.Annotations;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Ledger;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Entities.Documents.CycleCount;
using Nimbo.Wms.Domain.Entities.Ledger;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Application.Services.Documents;

[PublicAPI]
public sealed class CycleCountDocumentPostingService : IDocumentPostingService<CycleCountDocument>
{
    private readonly IStockLedgerEntryRepository _stockLedgerEntryRepo;
    private readonly IInventoryItemRepository _inventoryItemRepo;

    public CycleCountDocumentPostingService(
        IStockLedgerEntryRepository stockLedgerEntryRepo,
        IInventoryItemRepository inventoryItemRepo)
    {
        _stockLedgerEntryRepo = stockLedgerEntryRepo;
        _inventoryItemRepo = inventoryItemRepo;
    }

    public async Task PostAsync(CycleCountDocument document, CancellationToken ct = default)
    {
        if (document.Status is not CycleCountStatus.Completed)
            throw new DomainException("Document must be completed to post");

        foreach (var line in document.Lines)
        {
            if (!line.ActualQuantity.HasValue)
                continue;

            var inventoryItem = await _inventoryItemRepo.GetByCriteriaAsync(
                document.WarehouseId,
                line.LocationId,
                line.ItemId,
                ct);

            var actualQuantity = line.ActualQuantity.Value;
            if (inventoryItem is null)
            {
                if (actualQuantity.IsZero)
                    continue;

                inventoryItem = new InventoryItem(
                    InventoryItemId.New(),
                    line.ItemId,
                    document.WarehouseId,
                    line.LocationId,
                    Quantity.Zero(actualQuantity.Uom));

                await _inventoryItemRepo.AddAsync(inventoryItem, ct);
            }

            var delta = actualQuantity.GetDelta(inventoryItem.Quantity);
            if (delta.IsZero)
                continue;

            if (inventoryItem.Quantity.Value < Math.Abs(delta.Value))
                throw new DomainException($"Insufficient stock for Item {line.ItemId} at Location {line.LocationId}");

            inventoryItem.ApplyDelta(delta);

            var ledgerEntry = new StockLedgerEntry(
                inventoryItem.Id,
                inventoryItem.ItemId,
                inventoryItem.LocationId,
                inventoryItem.WarehouseId,
                delta,
                inventoryItem.Quantity, // Final authoritative balance
                document.Id,
                line.Id,
                LedgerTransactionType.CountingAdjustment,
                DateTime.UtcNow);

            await _stockLedgerEntryRepo.AddAsync(ledgerEntry, ct);

            if (inventoryItem.Quantity.IsZero)
                await _inventoryItemRepo.DeleteAsync(inventoryItem, ct);
        }

        document.Post();
    }
}
