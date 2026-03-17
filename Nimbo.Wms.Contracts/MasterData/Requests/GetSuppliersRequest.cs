using MediatR;
using Nimbo.Wms.Contracts.Topology.Dtos;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

public sealed record GetSuppliersRequest : IRequest<IReadOnlyList<SupplierDto>>;
