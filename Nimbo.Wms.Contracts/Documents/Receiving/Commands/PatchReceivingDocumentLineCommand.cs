using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Contracts.Documents.Receiving.Commands;

[PublicAPI]
public sealed record PatchReceivingDocumentLineCommand(
    Guid DocumentId,
    Guid Id,
    Guid? ToLocationId,
    QuantityDto? ReceivedQuantity,
    QuantityDto? ExpectedQuantity,
    string? Notes,
    long DocumentVersion
) : IRequest, ITxRequest;
