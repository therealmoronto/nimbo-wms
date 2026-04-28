using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Entities.Documents;

[PublicAPI]
public interface IDocumentPostedEvent : IDomainEvent
{
    string DocumentCode { get; }

    string DocumentTitle { get; }

    long DocumentVersion { get; }
}
