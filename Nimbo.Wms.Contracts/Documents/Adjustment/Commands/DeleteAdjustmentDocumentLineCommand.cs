using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Documents.Adjustment.Commands;

[PublicAPI]
public sealed record DeleteAdjustmentDocumentLineCommand(
    Guid DocumentId,
    Guid Id,
    long DocumentVersion
) : IRequest<Result>, ITxRequest;
