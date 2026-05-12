using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Stock.Dtos;

namespace Nimbo.Wms.Contracts.Stock.Requests;

[PublicAPI]
public sealed record GetBatchQuery(Guid BatchId) : IRequest<BatchDto>;
