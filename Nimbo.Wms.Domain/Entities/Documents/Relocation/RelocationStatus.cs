using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Entities.Documents.Relocation;

[PublicAPI]
public enum RelocationStatus
{
    Draft = 0,
    InProgress,
    Posted,
    Cancelled
}
