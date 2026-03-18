using MediatR;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Contracts.Stock.Requests;

public sealed record CreateBatchRequest(
    Guid ItemId,
    Guid? SupplierId,
    string BatchNumber,
    DateTimeOffset? ManufacturedAt,
    DateTimeOffset? ExpiryDate,
    DateTimeOffset? ReceivedAt,
    string? Notes
) : IRequest<BatchId>;

public sealed record CreateBatchResponse(Guid Id);
