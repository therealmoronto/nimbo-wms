using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Models.Documents.Shipment;

[PublicAPI]
public sealed record AddPickLineRequest(
    Guid ItemId,
    Guid FromLocationId,
    QuantityDto Quantity,
    string? Notes
);

[PublicAPI]
public sealed record AddPickLineResponse(Guid Id);
