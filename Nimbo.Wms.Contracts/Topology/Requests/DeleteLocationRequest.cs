using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Topology.Requests;

[PublicAPI]
public sealed record DeleteLocationRequest(Guid LocationGuid) : IRequest, ITxRequest;
