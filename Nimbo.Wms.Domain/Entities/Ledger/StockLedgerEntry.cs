using JetBrains.Annotations;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Entities.Ledger;

[PublicAPI]
public sealed class StockLedgerEntry : BaseEntity<StockLedgerEntryId>
{
    private StockLedgerEntry()
    {
        //  EF Core
    }

    public StockLedgerEntry(
        InventoryItemId inventoryItemId,
        ItemId itemId,
        LocationId locationId,
        WarehouseId warehouseId,
        QuantityDelta quantityDelta,
        Quantity balanceAfter,
        Guid sourceDocumentId,
        Guid sourceDocumentLineId,
        LedgerTransactionType transactionType,
        DateTime occurredAt)
    {
        Id = StockLedgerEntryId.New();
        InventoryItemId = inventoryItemId;
        ItemId = itemId;
        LocationId = locationId;
        WarehouseId = warehouseId;
        QuantityDelta = quantityDelta;
        BalanceAfter = balanceAfter;
        SourceDocumentId = sourceDocumentId;
        SourceDocumentLineId = sourceDocumentLineId;
        TransactionType = transactionType;
        OccurredAt = occurredAt;
    }

    public InventoryItemId InventoryItemId { get; }

    public ItemId ItemId { get; }

    public LocationId LocationId { get; }

    public WarehouseId WarehouseId { get; }

    public QuantityDelta QuantityDelta { get; }

    public Quantity BalanceAfter { get; }

    public Guid SourceDocumentId { get; }

    public Guid SourceDocumentLineId { get; }

    public LedgerTransactionType TransactionType { get; }

    public DateTime OccurredAt { get; }
}
