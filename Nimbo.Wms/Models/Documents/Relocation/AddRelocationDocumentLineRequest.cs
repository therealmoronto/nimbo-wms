using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Models.Documents.Relocation;

[PublicAPI]
public sealed record AddRelocationDocumentLineRequest(
    Guid ItemId,
    Guid FromLocationId,
    Guid ToLocationId,
    QuantityDto Quantity,
    string? Notes,
    long DocumentVersion
);
