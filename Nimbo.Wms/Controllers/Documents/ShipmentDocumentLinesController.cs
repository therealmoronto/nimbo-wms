using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts.Documents.Shipment.Commands;
using Nimbo.Wms.Contracts.Documents.Shipment.Dtos;
using Nimbo.Wms.Contracts.Documents.Shipment.Queries;
using Nimbo.Wms.Models.Documents.Shipment;

namespace Nimbo.Wms.Controllers.Documents;

[ApiController]
[Route("api/documents/shipment/{documentGuid:guid}/lines")]
public class ShipmentDocumentLinesController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Adds a new line to a shipment document.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the shipment document.</param>
    /// <param name="request">The details of the line to add, including item, requested quantity, and optional notes.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="IActionResult"/> containing the unique identifier of the newly created line
    /// and a status code of Created if successful.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Produces("application/json")]
    public async Task<IActionResult> AddLine(
        Guid documentGuid,
        [FromBody] AddShipmentDocumentLineRequest request,
        [FromQuery] long documentVersion,
        CancellationToken ct)
    {
        var command = new AddShipmentDocumentLineCommand(
            documentGuid,
            request.ItemId,
            request.RequestedQuantity,
            request.StockLotId,
            request.Notes,
            documentVersion);
        var lineGuid = await sender.Send(command, ct);

        return CreatedAtAction(
            actionName: nameof(GetLines),
            new { documentGuid = documentGuid },
            new AddShipmentDocumentLineResponse(lineGuid));
    }

    /// <summary>
    /// Retrieves all lines for a shipment document.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the shipment document.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="IActionResult"/> containing a list of shipment document lines.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<IReadOnlyList<ShipmentDocumentLineDto>> GetLines(Guid documentGuid, CancellationToken ct)
    {
        var query = new GetShipmentDocumentLinesQuery(documentGuid);
        return await sender.Send(query, ct);
    }

    /// <summary>
    /// Updates a shipment document line.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the shipment document.</param>
    /// <param name="lineGuid">The unique identifier of the line to update.</param>
    /// <param name="request">The details of the updates to apply to the line.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="IActionResult"/> with a status code of NoContent if successful.</returns>
    [HttpPatch("{lineGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchLine(
        Guid documentGuid,
        Guid lineGuid,
        [FromBody] PatchShipmentDocumentLineRequest request,
        CancellationToken ct)
    {
        var command = new PatchShipmentDocumentLineCommand(
            documentGuid,
            lineGuid,
            request.RequestedQuantity,
            request.Notes,
            request.DocumentVersion);
        await sender.Send(command, ct);
        return NoContent();
    }

    /// <summary>
    /// Deletes a line from a shipment document.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the shipment document.</param>
    /// <param name="lineGuid">The unique identifier of the line to delete.</param>
    /// <param name="documentVersion">The version of the document for concurrency control.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="IActionResult"/> with a status code of NoContent if successful.</returns>
    [HttpDelete("{lineGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLine(
        Guid documentGuid,
        Guid lineGuid,
        [FromQuery] long documentVersion,
        CancellationToken ct)
    {
        var command = new DeleteShipmentDocumentLineCommand(documentGuid, lineGuid, documentVersion);
        await sender.Send(command, ct);
        return NoContent();
    }
}