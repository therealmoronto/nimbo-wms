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
    /// <summary>
    /// Get warehouses flat list without zones and locations.
    /// Returns empty list if no warehouses exist.
    /// </summary>
    /// <response code="200">OK</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<WarehouseListItemDto>), StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<IReadOnlyList<WarehouseListItemDto>> GetWarehouses(
        [FromServices] IQueryHandler<GetWarehousesQuery, IReadOnlyList<WarehouseListItemDto>> handler,
        CancellationToken ct)
    {
        return await handler.HandleAsync(new GetWarehousesQuery(), ct);
    }

    /// <summary>
    /// Get warehouse topology by warehouseGuid.
    /// </summary>
    /// <response code="200">OK</response>
    /// <response code="404">Not found</response>
    /// <response code="400">Bad request</response>
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

    /// <summary>
    /// Create warehouse
    /// </summary>
    /// <response code="201">Created</response>
    /// <response code="400">Bad request</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreateWarehouseResponse), StatusCodes.Status201Created)]
    [Produces("application/json")]
    public async Task<ActionResult<CreateWarehouseResponse>> CreateWarehouse(
        [FromBody] CreateWarehouseRequest request,
        [FromServices] ICommandHandler<CreateWarehouseCommand, WarehouseId> handler,
        CancellationToken ct)
    {
        var command = new CreateWarehouseCommand(request);
        var warehouseId = await handler.HandleAsync(command, ct);

        return CreatedAtAction(
            actionName: nameof(WarehousesController.GetWarehouseTopology),
            controllerName: "Warehouses",
            routeValues: new { warehouseGuid = warehouseId.Value },
            value: new CreateWarehouseResponse(warehouseId.Value));
    }
    
    /// <summary>
    /// Update warehouse
    /// </summary>
    /// <response code="204">No content</response>
    /// <response code="404">Not found</response>
    /// <response code="400">Bad request</response>
    [HttpPatch("{warehouseGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateWarehouse(
        [FromRoute] Guid warehouseGuid,
        [FromBody] PatchWarehouseRequest request,
        [FromServices] ICommandHandler<PatchWarehouseCommand> handler,
        CancellationToken ct)
    {
        var warehouseId = WarehouseId.From(warehouseGuid);
        await handler.HandleAsync(new PatchWarehouseCommand(warehouseId, request), ct);
        return NoContent();
    }

    /// <summary>
    /// Delete warehouse
    /// </summary>
    /// <response code="204">No content</response>
    /// <response code="404">Not found</response>
    /// <response code="400">Bad request</response>
    [HttpDelete("{warehouseGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteWarehouse(
        [FromRoute] Guid warehouseGuid,
        [FromServices] ICommandHandler<DeleteWarehouseCommand> handler,
        CancellationToken ct)
    {
        var warehouseId = WarehouseId.From(warehouseGuid);
        await handler.HandleAsync(new DeleteWarehouseCommand(warehouseId), ct);
        return NoContent();
    }
}
