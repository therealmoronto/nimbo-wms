using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Documents.Adjustment.Commands;

[PublicAPI]
public sealed record PatchAdjustmentDocumentCommand(
    Guid Id,
    string? Code,
    string? Title,
    string? ReasonCode,
    string? ReasonText,
    long Version
) : IRequest, ITxRequest;
