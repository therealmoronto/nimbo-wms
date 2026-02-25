using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Entities.Documents.CycleCount;

[PublicAPI]
public enum CycleCountStatus
{
    Draft = 0,
    Counting,
    Completed,
    Posted,
    Cancelled,
}
