using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Contracts.Documents.Receiving.Dtos;

[PublicAPI]
public sealed record ReceivingDocumentLineDto(
    Guid Id,
    Guid DocumentId,
    Guid ToLocationId,
    QuantityDto RecievedQuantity,
    QuantityDto? ExpectedQuantity,
    string? Notes
);
