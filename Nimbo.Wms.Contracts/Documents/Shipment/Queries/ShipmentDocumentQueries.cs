using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Documents.Shipment.Dtos;

namespace Nimbo.Wms.Contracts.Documents.Shipment.Queries;

[PublicAPI]
public sealed record GetShipmentDocumentQuery(Guid Id) : IRequest<ShipmentDocumentDto>;

[PublicAPI]
public sealed record GetShipmentDocumentsQuery : IRequest<IReadOnlyList<ShipmentDocumentBodyDto>>;

[PublicAPI]
public sealed record GetShipmentDocumentLinesQuery(Guid DocumentId) : IRequest<IReadOnlyList<ShipmentDocumentLineDto>>;

[PublicAPI]
public sealed record GetShipmentPickLinesQuery(Guid DocumentId) : IRequest<IReadOnlyList<ShipmentPickLineDto>>;
