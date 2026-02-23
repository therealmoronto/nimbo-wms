using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;
using Nimbo.Wms.Application.Abstractions.UseCases.Topology.Queries;
using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Contracts.Topology.Http;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Controllers.Topology;

[ApiController]
[Route("api/topology/warehouses")]
public sealed class WarehousesController : ControllerBase
{
    /// <summary>
    /// Retrieves a list of all warehouses with their basic details.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation, containing a read-only list of warehouse items with their identifiers, codes, and names.
    /// </returns>
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
    /// Retrieves the topology of a specific warehouse identified by its unique identifier.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation, containing the topology details of the warehouse,
    /// including zones and locations, if found. If no warehouse is found with the given identifier,
    /// a Not Found (404) response is returned.
    /// </returns>
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
    /// Creates a new warehouse using the specified details provided in the request model.
    /// </summary>
    /// <returns>
    /// An action result containing the details of the newly created warehouse, including its unique identifier,
    /// and a Created (201) response status.
    /// </returns>
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
    /// Updates the details of an existing warehouse identified by the specified unique identifier.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. If successful, it returns a No Content (204) response.
    /// </returns>
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
    /// Deletes a warehouse identified by the specified unique identifier.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. If successful, it returns a No Content (204) response.
    /// </returns>
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
