using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Documents.Adjustment.Commands;

[PublicAPI]
public sealed record PatchAdjustmentDocumentCommand(
    Guid Id,
    string? Code,
    string? Title,
    string? ReasonCode,
    string? ReasonText,
    long Version
) : IRequest<Result>, ITxRequest;
