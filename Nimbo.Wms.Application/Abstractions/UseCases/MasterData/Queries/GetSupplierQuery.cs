using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Queries;

public sealed record GetSupplierQuery(SupplierId SupplierId) : IQuery<SupplierDto>;
