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
    /// Create item
    /// </summary>
    /// <response code="201">Created</response>
    /// <response code="400">Bad request</response>
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
    /// Get all items
    /// </summary>
    /// <response code="200">OK</response>
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
    /// Get item by itemGuid
    /// </summary>
    /// <response code="200">OK</response>
    /// <response code="404">Not found</response>
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
    /// Patch item
    /// </summary>
    /// <response code="204">No content</response>
    /// <response code="404">Not found</response>
    /// <response code="400">Bad request</response>
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
    /// Delete item
    /// </summary>
    /// <response code="204">No content</response>
    /// <response code="404">Not found</response>
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
