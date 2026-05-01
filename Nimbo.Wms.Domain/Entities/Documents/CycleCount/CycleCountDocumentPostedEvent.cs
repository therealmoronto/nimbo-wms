using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Entities.Documents.CycleCount;

/// <summary>
/// Event that is raised when a cycle count document is posted.
/// </summary>
[PublicAPI]
public class CycleCountDocumentPostedEvent : IDocumentPostedEvent
{
    public CycleCountDocumentPostedEvent(Guid aggregateId, string documentCode, string documentTitle, long documentVersion)
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
