using Nimbo.Wms.Domain.Entities.Documents.Common;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Entities.Documents.Relocation;

public class RelocationDocumentLine : DocumentLineBase<RelocationDocumentId>
{
    private RelocationDocumentLine()
    {
        // Required by EF Core
    }

    public RelocationDocumentLine(RelocationDocumentId documentId, ItemId itemId, LocationId from, LocationId to, Quantity quantity, string? notes)
        : base(documentId, itemId, quantity, notes)
    {
        From = from;
        To = to;
    }

    public LocationId From { get; private set; }

    public LocationId To { get; private set; }

    public void ChangeFrom(LocationId from) => From = from;

    public void ChangeTo(LocationId to) => To = to;
}
