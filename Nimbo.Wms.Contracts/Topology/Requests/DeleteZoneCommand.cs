using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Topology.Requests;

[PublicAPI]
public sealed record DeleteZoneCommand(Guid ZoneGuid) : IRequest, ITxRequest;
