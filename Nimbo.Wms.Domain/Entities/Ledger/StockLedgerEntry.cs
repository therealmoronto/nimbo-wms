using JetBrains.Annotations;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Entities.Ledger;

[PublicAPI]
public sealed class StockLedgerEntry<TDocumentId> : BaseEntity<StockLedgerEntryId>
    where TDocumentId : struct, IEntityId
{
    private StockLedgerEntry()
    {
        //  EF Core
    }

    public StockLedgerEntry(
        InventoryItemId inventoryItemId,
        ItemId itemId,
        LocationId locationId,
        QuantityDelta quantityDelta,
        Quantity balanceAfter,
        TDocumentId sourceDocumentId,
        Guid sourceDocumentLineId,
        LedgerTransactionType transactionType,
        DateTime occurredAt)
    {
        Id = StockLedgerEntryId.New();
        InventoryItemId = inventoryItemId;
        ItemId = itemId;
        LocationId = locationId;
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

    public QuantityDelta QuantityDelta { get; }

    public Quantity BalanceAfter { get; }

    public TDocumentId SourceDocumentId { get; }

    public Guid SourceDocumentLineId { get; }

    public LedgerTransactionType TransactionType { get; }

    public DateTime OccurredAt { get; }
}
