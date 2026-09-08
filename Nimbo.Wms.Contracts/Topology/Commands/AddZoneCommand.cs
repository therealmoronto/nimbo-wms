using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Topology.Commands;

[PublicAPI]
public sealed record AddZoneCommand(
    Guid WarehouseGuid,
    string Code,
    string Name,
    string Type
) : IRequest<Result<Guid>>, ITxRequest;

