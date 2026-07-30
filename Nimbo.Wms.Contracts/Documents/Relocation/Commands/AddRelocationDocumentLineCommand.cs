using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Contracts.Documents.Relocation.Commands;

[PublicAPI]
public sealed record AddRelocationDocumentLineCommand(
    Guid DocumentId,
    Guid ItemId,
    Guid? BatchId,
    Guid FromLocationId,
    Guid ToLocationId,
    QuantityDto Quantity,
    string? Notes,
    long DocumentVersion
) : IRequest<Guid>, ITxRequest;
