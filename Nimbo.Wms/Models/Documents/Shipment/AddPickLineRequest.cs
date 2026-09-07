using JetBrains.Annotations;
using Nimbo.Wms.Contracts.ValueObject;

namespace Nimbo.Wms.Models.Documents.Shipment;

[PublicAPI]
public sealed record AddPickLineRequest(
    Guid ItemId,
    Guid FromLocationId,
    Guid StockLotId,
    QuantityDto Quantity,
    string? Notes
);

[PublicAPI]
public sealed record AddPickLineResponse(Guid Id);
