using JetBrains.Annotations;
using Nimbo.Wms.Contracts.ValueObject;

namespace Nimbo.Wms.Models.Documents.Shipment;

[PublicAPI]
public sealed record AddShipmentDocumentLineRequest(
    Guid ItemId,
    QuantityDto RequestedQuantity,
    Guid? StockLotId,
    string? Notes
);

[PublicAPI]
public sealed record AddShipmentDocumentLineResponse(Guid Id);
