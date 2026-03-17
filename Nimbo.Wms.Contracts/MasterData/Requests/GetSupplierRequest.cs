using MediatR;
using Nimbo.Wms.Contracts.Topology.Dtos;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

public sealed record GetSupplierRequest(Guid SupplierId) : IRequest<SupplierDto>;
