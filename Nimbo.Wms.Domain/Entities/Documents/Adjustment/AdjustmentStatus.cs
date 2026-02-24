using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Entities.Documents.Adjustment;

[PublicAPI]
public enum AdjustmentStatus
{
    Draft = 0,
    Approved,
    Posted,
    Cancelled
}
