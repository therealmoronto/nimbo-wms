using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Topology.Dtos;

namespace Nimbo.Wms.Contracts.Topology.Requests;

[PublicAPI]
public sealed record GetWarehouseTopologyQuery(Guid WarehouseId) : IRequest<WarehouseTopologyDto>;
