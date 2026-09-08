using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.ValueObject;

namespace Nimbo.Wms.Contracts.Documents.Relocation.Commands;

[PublicAPI]
public sealed record PatchRelocationDocumentLineCommand(
    Guid DocumentId,
    Guid Id,
    Guid? FromLocationId,
    Guid? ToLocationId,
    QuantityDto? Quantity,
    string? Notes,
    long DocumentVersion
) : IRequest<Result>, ITxRequest;
