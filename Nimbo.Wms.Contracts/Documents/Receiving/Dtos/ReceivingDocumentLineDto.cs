using JetBrains.Annotations;
using Nimbo.Wms.Contracts.ValueObject;

namespace Nimbo.Wms.Contracts.Documents.Receiving.Dtos;

[PublicAPI]
public sealed record ReceivingDocumentLineDto(
    Guid Id,
    Guid DocumentId,
    Guid ToLocationId,
    Guid ItemId,
    QuantityDto ReceivedQuantity,
    QuantityDto? ExpectedQuantity,
    string? Notes
);
