using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.UseCases.Stock.Commands;
using Nimbo.Wms.Application.Abstractions.UseCases.Stock.Queries;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Contracts.Stock.Http;
using Nimbo.Wms.Domain.Identification;

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
    public async Task<IActionResult> CreateInventoryItem(
        [FromBody] CreateInventoryItemRequest request,
        [FromServices] ICommandHandler<CreateInventoryItemCommand, InventoryItemId> handler,
        CancellationToken ct)
    {
        var invetoryItemId = await handler.HandleAsync(new CreateInventoryItemCommand(request), ct);
        return CreatedAtAction(
            nameof(GetInventoryItem),
            "InventoryItems",
            new { inventoryItemGuid = invetoryItemId.Value },
            new CreateInventoryItemResponse(invetoryItemId.Value));
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
        [FromServices] IQueryHandler<GetInventoryItemQuery, InventoryItemDto> handler,
        CancellationToken ct)
    {
        var query = new GetInventoryItemQuery(InventoryItemId.From(inventoryItemGuid));
        return await handler.HandleAsync(query, ct);
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
        [FromServices] IQueryHandler<GetInventoryItemsQuery, IReadOnlyList<InventoryItemDto>> handler,
        CancellationToken ct)
    {
        var warehouseId = warehouseGuid is not null ? WarehouseId.From(warehouseGuid.Value) : (WarehouseId?)null;
        var itemId = itemGuid is not null ? ItemId.From(itemGuid.Value) : (ItemId?)null;
        var batchId = batchGuid is not null ? BatchId.From(batchGuid.Value) : (BatchId?)null;
        var query = new GetInventoryItemsQuery(warehouseId, itemId, batchId);
        return await handler.HandleAsync(query, ct);
    }
}
