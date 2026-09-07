using JetBrains.Annotations;
using Nimbo.Wms.Contracts.ValueObject;

namespace Nimbo.Wms.Contracts.Documents.Relocation.Dtos;

[PublicAPI]
public sealed record RelocationDocumentLineDto(
    Guid Id,
    Guid DocumentId,
    Guid ItemId,
    Guid FromLocationId,
    Guid ToLocationId,
    QuantityDto Quantity,
    string? Notes
);
