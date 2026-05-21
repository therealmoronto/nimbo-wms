using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Documents.Shipment.Commands;

[PublicAPI]
public sealed record DeleteShipmentDocumentLineCommand(
    Guid DocumentId,
    Guid Id,
    long DocumentVersion
) : IRequest, ITxRequest;