using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Documents.Receiving.Commands;

[PublicAPI]
public sealed record PatchReceivingDocumentLineCommand(
    Guid Id,
    Guid? ToLocationId,
    decimal? RecievedQuantity,
    decimal? ExpectedQuantity,
    string? Uom,
    string? Notes,
    long DocumentVersion
) : IRequest, ITxRequest;
