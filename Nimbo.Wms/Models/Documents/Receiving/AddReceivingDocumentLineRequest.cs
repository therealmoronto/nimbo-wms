using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Models.Documents.Receiving;

[PublicAPI]
public sealed record AddReceivingDocumentLineRequest(
    Guid ReceivingDocumentId,
    Guid ItemId,
    DateTimeOffset? ExpiryDate,
    string? BatchNumber,
    Guid ToLocationId,
    QuantityDto ExpectedQuantity,
    string? Notes,
    long DocumentVersion
);

[PublicAPI]
public sealed record AddReceivingDocumentLineResponse(Guid Id);
