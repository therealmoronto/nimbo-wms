using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Domain.Documents;

public abstract class Document<TId, TStatus> : IEntity<TId>
    where TId : struct, IEntityId
    where TStatus : struct, Enum
{
    protected Document(TId id,TStatus status,  DateTime createdAt, string? externalReference)
    {
        Id = id;
        CreatedAt = createdAt;
        Status = status;
        ExternalReference = !string.IsNullOrWhiteSpace(externalReference) ? externalReference.Trim() : null;
    }

    public TId Id { get; }

    public DateTime CreatedAt { get; }

    public TStatus Status { get; protected set; }

    public string? ExternalReference { get; }
}
