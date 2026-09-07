using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Topology.Commands;

[PublicAPI]
public sealed record DeleteWarehouseCommand(Guid WarehouseId) : IRequest, ITxRequest;
