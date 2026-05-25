using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Documents.Adjustment.Commands;

[PublicAPI]
public sealed record CreateAdjustmentDocumentCommand(
    Guid WarehouseId,
    string Code,
    string Title,
    string ReasonCode,
    string? ReasonText
) : IRequest<Guid>, ITxRequest;
