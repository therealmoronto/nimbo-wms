using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Domain.Entities.Documents.Common;

public abstract class DocumentLineBase<TDocumentId>
    where TDocumentId : struct, IEntityId
{
    protected DocumentLineBase()
    {
        // Required by EF Core
    }
    
    public DocumentLineBase(TDocumentId documentId, ItemId itemId, Quantity quantity, string? notes)
    {
        Id = Guid.NewGuid();
        DocumentId = documentId;
        ItemId = itemId;
        Quantity = quantity;
        Notes = notes?.Trim();
    }

    public Guid Id { get; }

    public TDocumentId DocumentId { get; }

    public ItemId ItemId { get; }

    public Quantity Quantity { get; private set; }

    public string? Notes { get; private set; }

    public void ChangeQuantity(Quantity quantity) => Quantity = quantity;

    public void ChangeNotes(string? notes) => Notes = notes?.Trim();
}
