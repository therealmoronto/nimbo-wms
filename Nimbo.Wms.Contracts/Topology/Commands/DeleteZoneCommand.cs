using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Topology.Commands;

[PublicAPI]
public sealed record DeleteZoneCommand(Guid ZoneGuid) : IRequest, ITxRequest;
