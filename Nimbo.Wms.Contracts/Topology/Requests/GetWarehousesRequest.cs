using MediatR;
using Nimbo.Wms.Contracts.Topology.Dtos;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Topology.Queries;

public sealed record GetWarehousesRequest : IRequest<IReadOnlyList<WarehouseListItemDto>>;
