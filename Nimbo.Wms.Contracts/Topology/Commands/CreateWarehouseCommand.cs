using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Topology.Commands;

[PublicAPI]
public sealed record CreateWarehouseCommand(
    string Code,
    string Name
) : IRequest<Guid>, ITxRequest;
