using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Topology.Requests;

[PublicAPI]
public sealed record DeleteLocationRequest(Guid LocationGuid) : IRequest;
