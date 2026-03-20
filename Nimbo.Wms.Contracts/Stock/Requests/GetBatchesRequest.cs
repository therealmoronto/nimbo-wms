using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Stock.Dtos;

namespace Nimbo.Wms.Contracts.Stock.Requests;

[PublicAPI]
public sealed record GetBatchesRequest(Guid? ItemId, Guid? SupplierId) : IRequest<IReadOnlyList<BatchDto>>;
