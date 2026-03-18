using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Stock.Dtos;

namespace Nimbo.Wms.Contracts.Stock.Requests;

[PublicAPI]
public sealed record GetBatchRequest(Guid BatchId) : IRequest<BatchDto>;
