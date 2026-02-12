using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;
using Nimbo.Wms.Application.Abstractions.UseCases.Topology.Queries;
using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Contracts.Topology.Http;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Controllers;

[ApiController]
[Route("api/topology/warehouses")]
public sealed class WarehousesController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<WarehouseListItemDto>), StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<IReadOnlyList<WarehouseListItemDto>> GetWarehouses(
        [FromServices] IQueryHandler<GetWarehousesQuery, IReadOnlyList<WarehouseListItemDto>> handler,
        CancellationToken ct)
    {
        return await handler.HandleAsync(new GetWarehousesQuery(), ct);
    }

    [HttpGet("{warehouseGuid:guid}")]
    [ProducesResponseType(typeof(WarehouseTopologyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<WarehouseTopologyDto> GetWarehouseTopology(
        [FromRoute] Guid warehouseGuid,
        [FromServices] IQueryHandler<GetWarehouseTopologyQuery, WarehouseTopologyDto> handler,
        CancellationToken ct)
    {
        WarehouseId warehouseId = WarehouseId.From(warehouseGuid);
        return await handler.HandleAsync(new GetWarehouseTopologyQuery(warehouseId), ct);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateWarehouseResponse), StatusCodes.Status201Created)]
    [Produces("application/json")]
    public async Task<ActionResult<CreateWarehouseResponse>> CreateWarehouse(
        [FromBody] CreateWarehouseRequest request,
        [FromServices] ICommandHandler<CreateWarehouseCommand, WarehouseId> handler,
        CancellationToken ct)
    {
        var command = new CreateWarehouseCommand(request.Code, request.Name);
        var warehouseId = await handler.HandleAsync(command, ct);

        return CreatedAtAction(
            actionName: nameof(CreateWarehouse),
            controllerName: "Warehouses",
            routeValues: new { warehouseId = warehouseId.Value },
            value: new CreateWarehouseResponse(warehouseId.Value));
    }
}
