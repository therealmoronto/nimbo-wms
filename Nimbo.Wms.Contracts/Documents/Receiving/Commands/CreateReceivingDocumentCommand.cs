using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Documents.Receiving.Commands;

[PublicAPI]
public sealed record CreateReceivingDocumentCommand(
    Guid WarehouseId,
    Guid SupplierId,
    string Code,
    string Title,
    string? Notes
) : IRequest<Guid>, ITxRequest;
