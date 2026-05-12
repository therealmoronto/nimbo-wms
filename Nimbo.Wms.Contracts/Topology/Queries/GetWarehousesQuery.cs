using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Topology.Dtos;

namespace Nimbo.Wms.Contracts.Topology.Queries;

[PublicAPI]
public sealed record GetWarehousesQuery : IRequest<IReadOnlyList<WarehouseListItemDto>>;
