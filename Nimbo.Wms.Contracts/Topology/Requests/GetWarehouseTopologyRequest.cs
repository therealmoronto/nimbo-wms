using MediatR;
using Nimbo.Wms.Contracts.Topology.Dtos;

namespace Nimbo.Wms.Contracts.Topology.Requests;

public sealed record GetWarehouseTopologyRequest(Guid WarehouseId) : IRequest<WarehouseTopologyDto>;
