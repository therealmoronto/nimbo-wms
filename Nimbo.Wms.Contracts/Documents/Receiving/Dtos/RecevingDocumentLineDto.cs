using JetBrains.Annotations;

namespace Nimbo.Wms.Contracts.Documents.Receiving.Dtos;

[PublicAPI]
public sealed record RecevingDocumentLineDto(
    Guid Id,
    Guid DocumentId,
    Guid ToLocationId,
    decimal RecievedQuantity,
    decimal? ExpectedQuantity,
    string Uom,
    string? Notes
);
