using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Contracts.Topology.Requests;
using Nimbo.Wms.Models.Topology;

namespace Nimbo.Wms.Controllers.Topology;

[ApiController]
[Route("api/topology/warehouses")]
public sealed class WarehousesController(ISender sender) : ControllerBase
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
    public async Task<IReadOnlyList<WarehouseListItemDto>> GetWarehouses(CancellationToken ct)
    {
        return await sender.Send(new GetWarehousesQuery(), ct);
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
    public async Task<WarehouseTopologyDto> GetWarehouseTopology([FromRoute] Guid warehouseGuid, CancellationToken ct)
    {
        return await sender.Send(new GetWarehouseTopologyQuery(warehouseGuid), ct);
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
    public async Task<ActionResult<CreateWarehouseResponse>> CreateWarehouse([FromBody] CreateWarehouseRequest request, CancellationToken ct)
    {
        var command = new CreateWarehouseCommand(request.Code, request.Name);
        var warehouseGuid = await sender.Send(command, ct);

        return CreatedAtAction(
            actionName: nameof(WarehousesController.GetWarehouseTopology),
            controllerName: "Warehouses",
            routeValues: new { warehouseGuid = warehouseGuid },
            value: new CreateWarehouseResponse(warehouseGuid));
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
    public async Task<IActionResult> UpdateWarehouse([FromRoute] Guid warehouseGuid, [FromBody] PatchWarehouseRequest request, CancellationToken ct)
    {
        var command = new PatchWarehouseCommand(warehouseGuid, request.Code, request.Name, request.Address, request.Description);
        await sender.Send(command, ct);
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
    public async Task<IActionResult> DeleteWarehouse([FromRoute] Guid warehouseGuid, CancellationToken ct)
    {
        await sender.Send(new DeleteWarehouseCommand(warehouseGuid), ct);
        return NoContent();
    }
}
