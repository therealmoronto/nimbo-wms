using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts.Stock.Commands;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Models.Stock;

namespace Nimbo.Wms.Controllers.Stock;

[ApiController]
[Route("api/stock")]
public class InventoryItemsController : ControllerBase
{
    /// <summary>
    /// Creates a new inventory item in the stock system.
    /// </summary>
    /// <remarks>
    /// This method processes the provided request to create an inventory item and stores it in the system.
    /// It returns a reference to the newly created inventory item if the operation is successful.
    /// </remarks>
    /// <returns>
    /// An <see cref="IActionResult"/> containing a 201 Created response with the inventory item ID
    /// if the operation is successful, or a 400 Bad Request response if the request validation fails.
    /// </returns>
    [HttpPost("inventory-items")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    [Obsolete("Prohibited for production use.")]
    public async Task<IActionResult> CreateInventoryItem(
        [FromBody] CreateInventoryItemRequest request,
        [FromServices] IMediator mediator,
        CancellationToken ct)
    {
        var command = new CreateInventoryItemCommand(request.ItemId, request.WarehouseId, request.LocationId, request.Quantity, request.QuantityUom, request.Status, request.BatchId, request.SerialNumber, request.UnitCost);
        var inventoryItemGuid = await mediator.Send(command, ct);
        return CreatedAtAction(
            nameof(GetInventoryItem),
            "InventoryItems",
            new { inventoryItemGuid = inventoryItemGuid },
            new CreateInventoryItemResponse(inventoryItemGuid));
    }

    /// <summary>
    /// Retrieves an inventory item by its unique identifier.
    /// </summary>
    /// <remarks>
    /// This method queries the stock system to retrieve details of a specific inventory item
    /// based on the provided GUID. If the item is found, it returns 200 OK with the item's details.
    /// Otherwise, it returns 404 Not Found if the item does not exist.
    /// </remarks>
    /// <returns>
    /// An <see cref="InventoryItemDto"/> containing data for the requested inventory item.
    /// Returns 200 OK if the item is found, or 404 Not Found if the item does not exist.
    /// </returns>
    [HttpGet("inventory-items/{inventoryItemGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<InventoryItemDto> GetInventoryItem(
        [FromRoute] Guid inventoryItemGuid,
        [FromServices] IMediator mediator,
        CancellationToken ct)
    {
        var query = new GetInventoryItemQuery(inventoryItemGuid);
        return await mediator.Send(query, ct);
    }

    /// <summary>
    /// Retrieves a list of inventory items based on specified criteria.
    /// </summary>
    /// <remarks>
    /// This method processes the provided filter request to fetch inventory items
    /// matching the given warehouse and item identifiers, if specified.
    /// Returns the list of inventory items or an empty list if no matches are found.
    /// </remarks>
    /// <returns>
    /// A list containing the retrieved inventory items.
    /// Returns an empty list if no matches are found.
    /// </returns>
    [HttpGet("inventory-items")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<IReadOnlyList<InventoryItemDto>> GetInventoryItems(
        [FromQuery] Guid? warehouseGuid,
        [FromQuery] Guid? itemGuid,
        [FromQuery] Guid? batchGuid,
        [FromServices] IMediator mediator,
        CancellationToken ct)
    {
        var query = new GetInventoryItemsQuery(warehouseGuid, itemGuid, batchGuid);
        return await mediator.Send(query, ct);
    }
}
