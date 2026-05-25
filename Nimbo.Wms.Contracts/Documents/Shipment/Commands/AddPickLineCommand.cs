using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Contracts.Documents.Shipment.Commands;

[PublicAPI]
public sealed record AddPickLineCommand(
    Guid DocumentId,
    Guid ItemId,
    Guid FromLocationId,
    QuantityDto Quantity,
    string? Notes,
    long DocumentVersion
) : IRequest<Guid>, ITxRequest;