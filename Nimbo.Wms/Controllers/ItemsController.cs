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

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<IReadOnlyList<ItemDto>> GetItems(
        [FromServices] IQueryHandler<GetItemsQuery, IReadOnlyList<ItemDto>> handler,
        CancellationToken ct)
    {
        return await handler.HandleAsync(new GetItemsQuery(), ct);
    }

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
