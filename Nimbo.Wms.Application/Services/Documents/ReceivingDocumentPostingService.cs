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
    private readonly IVendorLotRepository _vendorLotRepo;
    private readonly IStockLotRepository _stockLotRepo;

    public ReceivingDocumentPostingService(
        IStockLedgerEntryRepository stockLedgerEntryRepo,
        IItemRepository itemRepo,
        IInventoryItemRepository inventoryItemRepo,
        IVendorLotRepository vendorLotRepo,
        IStockLotRepository stockLotRepo)
    {
        _stockLedgerEntryRepo = stockLedgerEntryRepo;
        _itemRepo = itemRepo;
        _inventoryItemRepo = inventoryItemRepo;
        _vendorLotRepo = vendorLotRepo;
        _stockLotRepo = stockLotRepo;
    }

    public async Task PostAsync(ReceivingDocument document, CancellationToken ct = default)
    {
        if (document.Status is not ReceivingStatus.InProgress)
            throw new DomainException("Document must be in progress to post");

        foreach (var line in document.Lines)
        {
            if (line.ReceivedQuantity.IsZero)
                continue;

            var item = await _itemRepo.GetByIdAsync(line.ItemId, ct);
            if (item is null)
                throw new DomainException($"Item {line.ItemId} not found");

            VendorLotId? vendorLotId = null;
            if (item.IsBatchManaged)
            {
                if (string.IsNullOrWhiteSpace(line.BatchNumber))
                    throw new DomainException($"Batch number is required for item {line.ItemId}");

                var vendorLot = await _vendorLotRepo.FindOrCreateAsync(
                    line.ItemId, line.BatchNumber, document.SupplierId, line.ExpiryDate, ct);
                vendorLotId = vendorLot.Id;
            }

            // StockLot is always created — independent of IsBatchManaged — one per receiving line. This
            // is what makes FIFO/FEFO rotation possible for every item, not just batch-managed ones.
            var stockLot = new StockLot(StockLotId.New(), line.ItemId, document.Id, DateTime.UtcNow, vendorLotId);
            await _stockLotRepo.AddAsync(stockLot, ct);

            // A brand-new StockLotId can never already exist on an InventoryItem row, so this is always
            // a fresh row — never a find-then-reuse. Every receipt becomes its own trackable stock position.
            var inventoryItem = new InventoryItem(
                InventoryItemId.New(),
                line.ItemId,
                document.WarehouseId,
                line.ToLocationId,
                stockLot.Id,
                Quantity.Zero(line.ReceivedQuantity.Uom));

            await _inventoryItemRepo.AddAsync(inventoryItem, ct);

            inventoryItem.Increase(line.ReceivedQuantity);

            var ledgerEntity = new StockLedgerEntry(
                inventoryItem.Id,
                inventoryItem.ItemId,
                inventoryItem.StockLotId,
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
