using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Documents.Shipment.Commands;

[PublicAPI]
public sealed record CreateShipmentDocumentCommand(
    Guid WarehouseId,
    string Code,
    string Title
) : IRequest<Result<Guid>>, ITxRequest;
