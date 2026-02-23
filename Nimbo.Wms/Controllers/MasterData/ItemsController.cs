using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Commands;
using Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Queries;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Contracts.MasterData.Http;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Controllers;

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
        [FromBody]CreateItemRequest request,
        [FromServices] ICommandHandler<CreateItemCommand, ItemId> handler,
        CancellationToken ct)
    {
        var command = new CreateItemCommand(request);
        var itemId = await handler.HandleAsync(command, ct);
        
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
        [FromServices] IQueryHandler<GetItemsQuery, IReadOnlyList<ItemDto>> handler,
        CancellationToken ct)
    {
        return await handler.HandleAsync(new GetItemsQuery(), ct);
    }

    /// <summary>
    /// Retrieves an item by its unique identifier.
    /// </summary>
    /// <param name="itemGuid">The unique identifier of the item to retrieve.</param>
    /// <param name="handler">The query handler responsible for processing the request.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
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
        [FromServices] IQueryHandler<GetItemQuery, ItemDto> handler,
        CancellationToken ct)
    {
        var query = new GetItemQuery(ItemId.From(itemGuid));
        return await handler.HandleAsync(query, ct);
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
        [FromServices] ICommandHandler<PatchItemCommand> handler,
        CancellationToken ct)
    {
        var command = new PatchItemCommand(ItemId.From(itemGuid), request);
        await handler.HandleAsync(command, ct);
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
        [FromServices] ICommandHandler<DeleteItemCommand> handler,
        CancellationToken ct)
    {
        var command = new DeleteItemCommand(ItemId.From(itemGuid));
        await handler.HandleAsync(command, ct);
        return NoContent();
    }
}
