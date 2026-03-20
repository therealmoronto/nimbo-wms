using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Contracts.Topology.Requests;

[PublicAPI]
public sealed record CreateWarehouseRequest(
    string Code,
    string Name
) : IRequest<WarehouseId>;

[PublicAPI]
public sealed record CreateWarehouseResponse(Guid Id);
