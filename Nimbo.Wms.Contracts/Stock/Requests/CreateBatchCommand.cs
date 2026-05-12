using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Stock.Requests;

[PublicAPI]
public sealed record CreateBatchCommand(
    Guid ItemId,
    Guid? SupplierId,
    string BatchNumber,
    DateTimeOffset? ManufacturedAt,
    DateTimeOffset? ExpiryDate,
    DateTimeOffset? ReceivedAt,
    string? Notes
) : IRequest<Guid>, ITxRequest;
