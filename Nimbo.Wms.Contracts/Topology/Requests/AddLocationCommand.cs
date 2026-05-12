using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Topology.Requests;

[PublicAPI]
public sealed record AddLocationCommand(
    Guid WarehouseGuid,
    Guid ZoneGuid,
    string Code,
    string Type
) : IRequest<Guid>, ITxRequest;
