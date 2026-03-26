using JetBrains.Annotations;

namespace Nimbo.Wms.Infrastructure.Persistence.Outbox;

[PublicAPI]
public class OutboxMessage
{
    // ReSharper disable once EmptyConstructor
    public OutboxMessage()
    {
        // Required by EF Core
    }

    public Guid Id { get; } = Guid.NewGuid();

    public string Type { get; init; }

    public string Content { get; init; }

    public DateTime OccuredAt { get; } = DateTime.UtcNow;

    public DateTime? ProcessedAt { get; set; }

    public string? Error { get; set; }

    public int RetryCount { get; set; }

    public bool IsDeadLetter { get; set; }
}
