using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Contracts.Documents.Shipment.Commands;

[PublicAPI]
public sealed record AddShipmentDocumentLineCommand(
    Guid DocumentId,
    Guid ItemId,
    Guid? BatchId,
    QuantityDto RequestedQuantity,
    string? Notes,
    long DocumentVersion
) : IRequest<Guid>, ITxRequest;
