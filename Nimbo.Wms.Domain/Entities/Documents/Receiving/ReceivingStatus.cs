using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Entities.Documents.Receiving;

[PublicAPI]
public enum ReceivingStatus
{
    Draft = 0,
    InProgress,
    Posted,
    Cancelled
}
