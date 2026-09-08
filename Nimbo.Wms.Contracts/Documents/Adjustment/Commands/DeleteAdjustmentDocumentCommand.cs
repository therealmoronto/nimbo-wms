using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Documents.Adjustment.Commands;

[PublicAPI]
public record DeleteAdjustmentDocumentCommand(Guid Id, long Version) : IRequest<Result>, ITxRequest;
