using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Domain.Entities.Documents.Common;

public interface IDocument : IDomainEventsContainer
{
    IEntityId Id { get; }

    string Code { get; }

    string Title { get; }

    Enum Status { get; }

    DateTime CreatedAt { get; }

    DateTime UpdatedAt { get; }

    DateTime? PostedAt { get; }

    long Version { get; }

    string? Notes { get; }
}
