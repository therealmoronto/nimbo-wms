using JetBrains.Annotations;
using Nimbo.Wms.Contracts.ValueObject;

namespace Nimbo.Wms.Models.Documents.Relocation;

[PublicAPI]
public sealed record PatchRelocationDocumentLineRequest(
    Guid? FromLocationId,
    Guid? ToLocationId,
    QuantityDto? Quantity,
    string? Notes,
    long DocumentVersion
);
