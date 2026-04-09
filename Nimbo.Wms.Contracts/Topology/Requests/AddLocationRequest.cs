using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Topology.Requests;

[PublicAPI]
public sealed record AddLocationRequest(
    Guid WarehouseGuid,
    Guid ZoneGuid,
    string Code,
    string Type
) : IRequest<Guid>, ITxRequest;

[PublicAPI]
public sealed record AddLocationResponse(Guid LocationId);
