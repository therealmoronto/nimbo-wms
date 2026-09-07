using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Topology.Commands;

[PublicAPI]
public sealed record AddLocationCommand(
    Guid WarehouseGuid,
    Guid ZoneGuid,
    string Code,
    string Type
) : IRequest<Guid>, ITxRequest;
