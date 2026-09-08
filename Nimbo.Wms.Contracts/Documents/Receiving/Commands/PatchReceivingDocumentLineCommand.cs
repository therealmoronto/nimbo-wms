using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.ValueObject;

namespace Nimbo.Wms.Contracts.Documents.Receiving.Commands;

[PublicAPI]
public sealed record PatchReceivingDocumentLineCommand(
    Guid DocumentId,
    Guid Id,
    Guid? ToLocationId,
    QuantityDto? ExpectedQuantity,
    string? Notes,
    long DocumentVersion
) : IRequest<Result>, ITxRequest;
