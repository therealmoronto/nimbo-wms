using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Topology.Requests;

[PublicAPI]
public sealed record AddZoneCommand(
    Guid WarehouseGuid,
    string Code,
    string Name,
    string Type
) : IRequest<Guid>, ITxRequest;
