using MediatR;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;

public sealed record AddLocationRequest(
    Guid WarehouseGuid,
    Guid ZoneGuid,
    string Code,
    LocationType Type
) : IRequest<LocationId>;

public sealed record AddLocationResponse(Guid LocationId);
