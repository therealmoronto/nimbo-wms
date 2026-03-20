using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Contracts.Topology.Requests;

[PublicAPI]
public sealed record AddLocationRequest(
    Guid WarehouseGuid,
    Guid ZoneGuid,
    string Code,
    LocationType Type
) : IRequest<LocationId>;

[PublicAPI]
public sealed record AddLocationResponse(Guid LocationId);
