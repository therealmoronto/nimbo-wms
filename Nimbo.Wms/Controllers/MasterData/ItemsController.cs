using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Controllers.MasterData;

[ApiController]
[Route("api/items")]
public class ItemsController : ControllerBase
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
    public async Task<IActionResult> Createitem(
        [FromBody] CreateItemRequest request,
        [FromServices] IMediator mediator,
        CancellationToken ct)
    {
        var itemId = await mediator.Send(request, ct);
        return CreatedAtAction(
            actionName: nameof(GetItem),
            new { itemGuid = itemId.Value },
            new CreateItemResponse(itemId.Value));
    }

    /// <summary>
    /// Retrieves a list of all items.
    /// </summary>
    /// <returns>A read-only list of item data transfer objects.</returns>
    /// <response code="200">The items were successfully retrieved.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<IReadOnlyList<ItemDto>> GetItems(
        [FromServices] IMediator mediator,
        CancellationToken ct)
    {
        return await mediator.Send(new GetItemsRequest(), ct);
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
    public async Task<ItemDto> GetItem(
        [FromRoute] Guid itemGuid,
        [FromServices] IMediator mediator,
        CancellationToken ct)
    {
        var request = new GetItemRequest(ItemId.From(itemGuid));
        return await mediator.Send(request, ct);
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
    public async Task<IActionResult> PatchItem(
        [FromRoute] Guid itemGuid,
        [FromBody] PatchItemRequest request,
        [FromServices] IMediator mediator,
        CancellationToken ct)
    {
        await mediator.Send(request with { ItemGuid = itemGuid }, ct);
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
    public async Task<IActionResult> DeleteItem(
        [FromRoute] Guid itemGuid,
        [FromServices] IMediator mediator,
        CancellationToken ct)
    {
        var request = new DeleteItemRequest(itemGuid);
        await mediator.Send(request, ct);
        return NoContent();
    }
}
