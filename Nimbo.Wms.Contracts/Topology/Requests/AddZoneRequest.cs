using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Topology.Requests;

[PublicAPI]
public sealed record AddZoneRequest(
    Guid WarehouseGuid,
    string Code,
    string Name,
    string Type
) : IRequest<Guid>, ITxRequest;

[PublicAPI]
public sealed record AddZoneResponse(Guid ZoneId);
