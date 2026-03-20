using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Contracts.Topology.Requests;

[PublicAPI]
public sealed record AddZoneRequest(
    Guid WarehouseGuid,
    string Code,
    string Name,
    ZoneType Type
) : IRequest<ZoneId>, ITxRequest;

[PublicAPI]
public sealed record AddZoneResponse(Guid ZoneId);
