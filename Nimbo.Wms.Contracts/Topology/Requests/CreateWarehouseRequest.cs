using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Contracts.Topology.Requests;

[PublicAPI]
public sealed record CreateWarehouseRequest(
    string Code,
    string Name
) : IRequest<WarehouseId>, ITxRequest;

[PublicAPI]
public sealed record CreateWarehouseResponse(Guid Id);
