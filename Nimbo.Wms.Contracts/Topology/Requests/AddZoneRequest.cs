using MediatR;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;

public sealed record AddZoneRequest(
    Guid WarehouseGuid,
    string Code,
    string Name,
    ZoneType Type
) : IRequest<ZoneId>;

public sealed record AddZoneResponse(Guid ZoneId);
