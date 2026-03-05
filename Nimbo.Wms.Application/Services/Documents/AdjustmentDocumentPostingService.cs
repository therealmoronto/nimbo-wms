using JetBrains.Annotations;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Ledger;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Entities.Documents.Adjustment;
using Nimbo.Wms.Domain.Entities.Ledger;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Application.Services.Documents;

[PublicAPI]
public sealed class AdjustmentDocumentPostingService : IDocumentPostingService<AdjustmentDocument>
{
    private readonly IStockLedgerEntryRepository _stockLedgerEntryRepo;
    private readonly IInventoryItemRepository _inventoryItemRepo;

    public AdjustmentDocumentPostingService(
        IStockLedgerEntryRepository stockLedgerEntryRepo,
        IInventoryItemRepository inventoryItemRepo)
    {
        _stockLedgerEntryRepo = stockLedgerEntryRepo;
        _inventoryItemRepo = inventoryItemRepo;
    }

    public async Task PostAsync(AdjustmentDocument document, CancellationToken ct = default)
    {
        if (document.Status is not AdjustmentStatus.Approved)
            throw new DomainException("Document must be in progress to post");

        foreach (var line in document.Lines)
        {
            var inventoryItem = await _inventoryItemRepo.GetByCriteriaAsync(
                document.WarehouseId,
                line.LocationId,
                line.ItemId,
                ct);

            if (inventoryItem is null)
            {
                if (line.Delta < 0)
                    throw new DomainException($"Cannot adjust negative quantity for non-existent stock at {line.LocationId}");

                inventoryItem = new InventoryItem(
                    InventoryItemId.New(),
                    line.ItemId,
                    document.WarehouseId,
                    line.LocationId,
                    Quantity.Zero(line.Quantity.Uom));

                await _inventoryItemRepo.AddAsync(inventoryItem, ct);
            }

            if (inventoryItem.Quantity.Value < Math.Abs(line.Delta.Value))
                throw new DomainException($"Insufficient stock for Item {line.ItemId} at Location {line.LocationId}");

            inventoryItem.Quantity.ApplyDelta(line.Delta);

            var ledgerEntry = new StockLedgerEntry(
                inventoryItem.Id,
                inventoryItem.ItemId,
                inventoryItem.LocationId,
                inventoryItem.WarehouseId,
                line.Quantity.ToDelta(),
                inventoryItem.Quantity,
                document.Id,
                line.Id,
                LedgerTransactionType.ManualAdjustment,
                DateTime.UtcNow);

            await _stockLedgerEntryRepo.AddAsync(ledgerEntry, ct);

            if (inventoryItem.Quantity.IsZero)
                await _inventoryItemRepo.DeleteAsync(inventoryItem, ct);
        }

        document.Post();
    }
}
