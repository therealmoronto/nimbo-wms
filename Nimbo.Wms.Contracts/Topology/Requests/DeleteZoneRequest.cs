using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Topology.Requests;

[PublicAPI]
public sealed record DeleteZoneRequest(Guid ZoneGuid) : IRequest;
