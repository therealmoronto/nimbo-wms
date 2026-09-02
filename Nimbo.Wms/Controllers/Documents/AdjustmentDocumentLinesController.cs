using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts.Documents.Adjustment.Commands;
using Nimbo.Wms.Contracts.Documents.Adjustment.Dtos;
using Nimbo.Wms.Contracts.Documents.Adjustment.Queries;
using Nimbo.Wms.Models.Documents.Adjustment;

namespace Nimbo.Wms.Controllers.Documents;

[ApiController]
[Route("api/documents/adjustment/{documentGuid:guid}/lines")]
public class AdjustmentDocumentLinesController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Adds a line to an adjustment document with the specified details.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the adjustment document to which the line will be added.</param>
    /// <param name="request">The request containing the details of the line to be added, including item, location, delta, notes, and document version.</param>
    /// <param name="ct">The cancellation token used to cancel the asynchronous operation.</param>
    /// <returns>Returns an <see cref="IActionResult"/> indicating the result of the operation, with a created status and response containing the ID of the newly added line.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Produces("application/json")]
    public async Task<IActionResult> AddLine(
        Guid documentGuid,
        [FromBody] AddAdjustmentDocumentLineRequest request,
        CancellationToken ct)
    {
        var command = new AddAdjustmentDocumentLineCommand(
            documentGuid,
            request.ItemId,
            request.LocationId,
            request.Delta,
            request.StockLotId,
            request.Notes,
            request.DocumentVersion);
        var lineId = await sender.Send(command, ct);

        return Created(string.Empty, new AddAdjustmentDocumentLineResponse(lineId));
    }

    /// <summary>
    /// Retrieves the list of adjustment document lines associated with the specified document.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the adjustment document whose lines are being retrieved.</param>
    /// <param name="ct">The cancellation token used to cancel the asynchronous operation.</param>
    /// <returns>Returns a read-only list of <see cref="AdjustmentDocumentLineDto"/> objects representing the lines of the adjustment document.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<IReadOnlyList<AdjustmentDocumentLineDto>> GetLines(Guid documentGuid, CancellationToken ct)
    {
        var query = new GetAdjustmentDocumentLinesQuery(documentGuid);
        return await sender.Send(query, ct);
    }

    /// <summary>
    /// Updates an existing line in an adjustment document with the provided details.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the adjustment document containing the line to be updated.</param>
    /// <param name="lineGuid">The unique identifier of the line to be updated.</param>
    /// <param name="request">The request object containing the updated details for the line, including the location, delta, notes, and document version.</param>
    /// <param name="ct">The cancellation token used to cancel the asynchronous operation.</param>
    /// <returns>Returns an <see cref="IActionResult"/> indicating the result of the operation, with no content if successful or a not found status if the line does not exist.</returns>
    [HttpPatch("{lineGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchLine(
        Guid documentGuid,
        Guid lineGuid,
        [FromBody] PatchAdjustmentDocumentLineRequest request,
        CancellationToken ct)
    {
        var command = new PatchAdjustmentDocumentLineCommand(
            documentGuid,
            lineGuid,
            request.LocationId,
            request.Delta,
            request.Notes,
            request.DocumentVersion);
        await sender.Send(command, ct);
        return NoContent();
    }

    /// <summary>
    /// Deletes a specific line from an adjustment document based on the provided document and line identifiers.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the adjustment document from which the line will be deleted.</param>
    /// <param name="lineGuid">The unique identifier of the line to be deleted.</param>
    /// <param name="documentVersion">The version of the adjustment document to ensure consistency during deletion.</param>
    /// <param name="ct">The cancellation token used to cancel the asynchronous operation.</param>
    /// <returns>Returns an <see cref="IActionResult"/> indicating the result of the operation, with a no-content status if successful or not found if the line does not exist.</returns>
    [HttpDelete("{lineGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLine(
        Guid documentGuid,
        Guid lineGuid,
        [FromQuery] long documentVersion,
        CancellationToken ct)
    {
        var command = new DeleteAdjustmentDocumentLineCommand(documentGuid, lineGuid, documentVersion);
        await sender.Send(command, ct);
        return NoContent();
    }
}
