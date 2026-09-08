using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Documents.Shipment.Commands;

[PublicAPI]
public sealed record DeleteShipmentDocumentLineCommand(
    Guid DocumentId,
    Guid Id,
    long DocumentVersion
) : IRequest<Result>, ITxRequest;