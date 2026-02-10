using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Topology.Queries;

public sealed record GetWarehouseTopologyQuery (WarehouseId WarehouseId) : IQuery<WarehouseTopologyDto>;
