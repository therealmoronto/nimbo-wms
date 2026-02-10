using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Contracts.Topology.Dtos;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Topology.Queries;

public sealed record GetWarehousesQuery : IQuery<IReadOnlyList<WarehouseListItemDto>>;
