using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Contracts.Documents.Shipment.Commands;

[PublicAPI]
public sealed record PatchShipmentDocumentLineCommand(
    Guid DocumentId,
    Guid Id,
    QuantityDto? RequestedQuantity,
    string? Notes,
    long DocumentVersion
) : IRequest, ITxRequest;