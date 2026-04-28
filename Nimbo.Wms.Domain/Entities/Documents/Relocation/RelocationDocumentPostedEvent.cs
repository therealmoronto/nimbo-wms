using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Entities.Documents.Relocation;

/// <summary>
/// Event that is raised when a relocation document is posted.
/// </summary>
[PublicAPI]
public class RelocationDocumentPostedEvent : IDocumentPostedEvent
{
    public RelocationDocumentPostedEvent(Guid aggregateId, string documentCode, string documentTitle, long documentVersion)
    {
        AggregateId = aggregateId;
        DocumentCode = documentCode;
        DocumentTitle = documentTitle;
        DocumentVersion = documentVersion;
    }

    public Guid Id { get; } = Guid.NewGuid();

    public Guid AggregateId { get; }

    public DateTime OccurredAt { get; } = DateTime.UtcNow;

    public string DocumentCode { get; }

    public string DocumentTitle { get; }

    public long DocumentVersion { get; }
}
