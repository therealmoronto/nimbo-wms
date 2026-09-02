using JetBrains.Annotations;
using Nimbo.Wms.Domain.Entities.Documents.Common;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Entities.Documents.Relocation;

[PublicAPI]
public sealed class RelocationDocumentLine : DocumentLineBase<RelocationDocumentId>
{
    private RelocationDocumentLine()
    {
        // Required by EF Core
    }

    public RelocationDocumentLine(
        RelocationDocumentId documentId,
        ItemId itemId,
        LocationId from,
        LocationId to,
        Quantity quantity,
        StockLotId? stockLotId = null,
        string? notes = null)
        : base(documentId, itemId, quantity, notes)
    {
        From = from;
        To = to;
        StockLotId = stockLotId;
    }

    public LocationId From { get; private set; }

    public LocationId To { get; private set; }

    /// <summary>
    /// Which stock lot to relocate. Optional when only one lot exists at the source item/location;
    /// required once more than one lot coexists there (see IInventoryItemRepository.GetByCriteriaAsync).
    /// </summary>
    public StockLotId? StockLotId { get; private set; }

    public void ChangeFrom(LocationId from) => From = from;

    public void ChangeTo(LocationId to) => To = to;

    public void ChangeStockLotId(StockLotId? stockLotId) => StockLotId = stockLotId;
}
