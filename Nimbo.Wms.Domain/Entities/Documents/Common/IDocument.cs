using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Domain.Entities.Documents.Common;

public interface IDocument : IDomainEventsContainer
{
    public const int CodeMaxLength = 32;
    public const int TitleMaxLength = 128;
    public const int NotesMaxLength = 512;

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
