using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Documents.Shipment.Commands;

[PublicAPI]
public sealed record CreateShipmentDocumentCommand(
    Guid WarehouseId,
    string Code,
    string Title
) : IRequest<Guid>, ITxRequest;
