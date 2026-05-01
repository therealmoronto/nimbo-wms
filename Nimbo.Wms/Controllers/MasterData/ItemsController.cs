using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Contracts.MasterData.Requests;

namespace Nimbo.Wms.Controllers.MasterData;

[ApiController]
[Route("api/items")]
public class ItemsController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Creates a new item.
    /// </summary>
    /// <returns>An action result indicating the outcome of the operation.</returns>
    /// <response code="201">The item was successfully created.</response>
    /// <response code="400">The request is invalid.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Produces("application/json")]
    public async Task<IActionResult> Createitem([FromBody] CreateItemRequest request, CancellationToken ct)
    {
        var itemGuid = await sender.Send(request, ct);
        return CreatedAtAction(
            actionName: nameof(GetItem),
            new { itemGuid = itemGuid },
            new CreateItemResponse(itemGuid));
    }

    /// <summary>
    /// Retrieves a list of all items.
    /// </summary>
    /// <returns>A read-only list of item data transfer objects.</returns>
    /// <response code="200">The items were successfully retrieved.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<IReadOnlyList<ItemDto>> GetItems(CancellationToken ct)
    {
        return await sender.Send(new GetItemsRequest(), ct);
    }

    /// <summary>
    /// Retrieves an item by its unique identifier.
    /// </summary>
    /// <returns>The details of the requested item.</returns>
    /// <response code="200">The item was successfully retrieved.</response>
    /// <response code="404">The item with the specified identifier was not found.</response>
    [HttpGet]
    [Route("{itemGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<ItemDto> GetItem([FromRoute] Guid itemGuid, CancellationToken ct)
    {
        var request = new GetItemRequest(itemGuid);
        return await sender.Send(request, ct);
    }

    /// <summary>
    /// Updates an existing item with the specified details.
    /// </summary>
    /// <returns>An action result indicating the outcome of the operation.</returns>
    /// <response code="204">The item was successfully updated.</response>
    /// <response code="400">The request is invalid.</response>
    /// <response code="404">The specified item was not found.</response>
    [HttpPatch("{itemGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchItem([FromRoute] Guid itemGuid, [FromBody] PatchItemRequest request, CancellationToken ct)
    {
        await sender.Send(request with { ItemGuid = itemGuid }, ct);
        return NoContent();
    }

    /// <summary>
    /// Deletes an item identified by the specified GUID.
    /// </summary>
    /// <returns>An action result indicating the outcome of the operation.</returns>
    /// <response code="204">The item was successfully deleted.</response>
    /// <response code="404">The item was not found.</response>
    [HttpDelete("{itemGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteItem([FromRoute] Guid itemGuid, CancellationToken ct)
    {
        var request = new DeleteItemRequest(itemGuid);
        await sender.Send(request, ct);
        return NoContent();
    }
}
