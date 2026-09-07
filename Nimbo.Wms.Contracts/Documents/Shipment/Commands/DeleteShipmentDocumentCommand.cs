using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Documents.Shipment.Commands;

[PublicAPI]
public sealed record DeleteShipmentDocumentCommand(
    Guid Id,
    long Version
) : IRequest, ITxRequest;