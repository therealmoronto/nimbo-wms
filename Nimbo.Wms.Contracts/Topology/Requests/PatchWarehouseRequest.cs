using MediatR;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;

public sealed record PatchWarehouseRequest(
    Guid WarehouseGuid,
    string? Code = null,
    string? Name = null,
    string? Address = null,
    string? Description = null
) : IRequest;
