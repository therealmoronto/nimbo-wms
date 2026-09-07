using JetBrains.Annotations;
using Nimbo.Wms.Contracts.ValueObject;

namespace Nimbo.Wms.Models.Documents.Receiving;

[PublicAPI]
public sealed record AddReceivingDocumentLineRequest(
    Guid ReceivingDocumentId,
    Guid ItemId,
    Guid ToLocationId,
    QuantityDto ExpectedQuantity,
    DateTime? ExpiryDate,
    string? BatchNumber,
    string? Notes,
    long DocumentVersion
);

[PublicAPI]
public sealed record AddReceivingDocumentLineResponse(Guid Id);
