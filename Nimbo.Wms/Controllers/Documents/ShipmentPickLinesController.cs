using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts.Documents.Shipment.Commands;
using Nimbo.Wms.Contracts.Documents.Shipment.Dtos;
using Nimbo.Wms.Contracts.Documents.Shipment.Queries;
using Nimbo.Wms.Extensions;
using Nimbo.Wms.Models.Documents.Shipment;

namespace Nimbo.Wms.Controllers.Documents;

[ApiController]
[Route("api/documents/shipment/{documentGuid:guid}/pick-lines")]
public class ShipmentPickLinesController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Adds a new pick line to a shipment document.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the shipment document.</param>
    /// <param name="request">The details of the pick line to add, including item, location, and quantity.</param>
    /// <param name="documentVersion">The version of the document for concurrency control.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="IActionResult"/> containing the unique identifier of the newly created pick line.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Produces("application/json")]
    public async Task<IActionResult> AddPickLine(
        Guid documentGuid,
        [FromBody] AddPickLineRequest request,
        [FromQuery] long documentVersion,
        CancellationToken ct)
    {
        var command = new AddPickLineCommand(
            documentGuid,
            request.ItemId,
            request.FromLocationId,
            request.StockLotId,
            request.Quantity,
            request.Notes,
            documentVersion);
        var result = await sender.Send(command, ct);

        return result.ToActionResult(
            this,
            v => new AddPickLineResponse(v),
            nameof(GetPickLines),
            "ShipmentPickLines",
            result.IsSuccess ? new { documentGuid } : null);
    }

    /// <summary>
    /// Retrieves all pick lines for a shipment document.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the shipment document.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="IActionResult"/> containing a list of pick lines.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<IActionResult> GetPickLines(Guid documentGuid, CancellationToken ct)
    {
        var query = new GetShipmentPickLinesQuery(documentGuid);
        var result = await sender.Send(query, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Removes a pick line from a shipment document.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the shipment document.</param>
    /// <param name="pickLineGuid">The unique identifier of the pick line to remove.</param>
    /// <param name="documentVersion">The version of the document for concurrency control.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="IActionResult"/> with a status code of NoContent if successful.</returns>
    [HttpDelete("{pickLineGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemovePickLine(
        Guid documentGuid,
        Guid pickLineGuid,
        [FromQuery] long documentVersion,
        CancellationToken ct)
    {
        var command = new RemovePickLineCommand(documentGuid, pickLineGuid, documentVersion);
        var result = await sender.Send(command, ct);
        return result.ToActionResult(this);
    }
}
