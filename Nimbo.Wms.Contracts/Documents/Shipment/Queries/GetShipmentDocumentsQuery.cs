using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Documents.Shipment.Dtos;

namespace Nimbo.Wms.Contracts.Documents.Shipment.Queries;

[PublicAPI]
public sealed record GetShipmentDocumentsQuery : IRequest<Result<IReadOnlyList<ShipmentDocumentBodyDto>>>;
