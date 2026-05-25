using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Common.Dtos;

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
) : IRequest, ITxRequest;
