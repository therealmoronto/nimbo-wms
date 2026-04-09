using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Topology.Requests;

[PublicAPI]
public sealed record CreateWarehouseRequest(
    string Code,
    string Name
) : IRequest<Guid>, ITxRequest;

[PublicAPI]
public sealed record CreateWarehouseResponse(Guid Id);
