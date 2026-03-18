using MediatR;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;

public sealed record DeleteZoneRequest(Guid ZoneGuid) : IRequest;
