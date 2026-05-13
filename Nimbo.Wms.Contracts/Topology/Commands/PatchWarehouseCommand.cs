using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Topology.Commands;

[PublicAPI]
public sealed record PatchWarehouseCommand(
    Guid WarehouseGuid,
    string? Code = null,
    string? Name = null,
    string? Address = null,
    string? Description = null
) : IRequest, ITxRequest;
