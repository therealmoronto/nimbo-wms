using JetBrains.Annotations;

namespace Nimbo.Wms.Domain.Entities.Documents.Shipment;

[PublicAPI]
public enum ShipmentStatus
{
    Draft = 0,
    InProgress,
    Shipped,
    Posted,
    Cancelled
}
