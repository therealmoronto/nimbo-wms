using JetBrains.Annotations;
using Nimbo.Wms.Contracts.ValueObject;

namespace Nimbo.Wms.Models.Documents.Receiving;

[PublicAPI]
public sealed record PatchReceivingDocumentLineRequest(
    Guid? ToLocationId,
    QuantityDto? ExpectedQuantity,
    string? Notes,
    long DocumentVersion
);

[PublicAPI]
public sealed record PatchReceivingDocumentLineResponse(Guid Id);
