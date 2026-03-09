using JetBrains.Annotations;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Ledger;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Entities.Documents.Shipment;
using Nimbo.Wms.Domain.Entities.Ledger;

namespace Nimbo.Wms.Application.Services.Documents;

[PublicAPI]
public sealed class ShipmentDocumentPostingService : IDocumentPostingService<ShipmentDocument>
{
    private readonly IStockLedgerEntryRepository _stockLedgerEntryRepo;
    private readonly IInventoryItemRepository _inventoryItemRepo;

    public ShipmentDocumentPostingService(
        IStockLedgerEntryRepository stockLedgerEntryRepo,
        IInventoryItemRepository inventoryItemRepo)
    {
        _stockLedgerEntryRepo = stockLedgerEntryRepo;
        _inventoryItemRepo = inventoryItemRepo;
    }

    public async Task PostAsync(ShipmentDocument document, CancellationToken ct = default)
    {
        if (document.Status is not ShipmentStatus.InProgress)
            throw new DomainException("Shipment must be in progress to post");

        foreach (var pickLine in document.PickLines)
        {
            if (pickLine.Quantity.IsZero)
                continue;

            var inventoryItem = await _inventoryItemRepo.GetByCriteriaAsync(
                document.WarehouseId,
                pickLine.FromLocation,
                pickLine.ItemId,
                ct);

            if (inventoryItem is null || inventoryItem.Quantity.Value < pickLine.Quantity.Value)
            {
                throw new DomainException($"Insufficient stock for Item {pickLine.ItemId} at Location {pickLine.FromLocation}");
            }

            inventoryItem.Decrease(pickLine.Quantity);

            var ledgerEntry = new StockLedgerEntry(
                inventoryItem.Id,
                inventoryItem.ItemId,
                inventoryItem.LocationId,
                inventoryItem.WarehouseId,
                pickLine.Quantity.ToDelta().Negate(),
                inventoryItem.Quantity,
                document.Id,
                pickLine.Id,
                LedgerTransactionType.Shipment,
                DateTime.UtcNow
            );

            await _stockLedgerEntryRepo.AddAsync(ledgerEntry, ct);
        }

        document.Post();
    }
}
