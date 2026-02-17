using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Contracts.Topology.Dtos;

namespace Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Queries;

public sealed record GetSupplierQuery(Guid SupplierId) : IQuery<SupplierDto>;
