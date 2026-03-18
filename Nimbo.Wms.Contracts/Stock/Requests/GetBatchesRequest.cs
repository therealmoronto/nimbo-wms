using MediatR;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Contracts.Stock.Requests;

public sealed record GetBatchesRequest(Guid? ItemId, Guid? SupplierId) : IRequest<IReadOnlyList<BatchDto>>;
