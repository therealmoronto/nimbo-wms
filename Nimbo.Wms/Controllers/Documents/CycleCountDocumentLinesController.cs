using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts.Documents.CycleCount.Commands;
using Nimbo.Wms.Contracts.Documents.CycleCount.Dtos;
using Nimbo.Wms.Contracts.Documents.CycleCount.Queries;
using Nimbo.Wms.Extensions;
using Nimbo.Wms.Models.Documents.CycleCount;

namespace Nimbo.Wms.Controllers.Documents;

[ApiController]
[Route("api/documents/cycle-count/{documentGuid:guid}/lines")]
public class CycleCountDocumentLinesController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Adds a new line to the cycle count document.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the cycle count document to which the line is being added.</param>
    /// <param name="request">The request object containing the details of the line to be added, such as the item,
    /// location, expected quantity, notes, and document version.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an
    /// <see cref="AddCycleCountDocumentLineResponse"/> object with the unique identifier of the added line.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Produces("application/json")]
    public async Task<IActionResult> AddLine(
        Guid documentGuid,
        [FromBody] AddCycleCountDocumentLineRequest request,
        CancellationToken ct)
    {
        var command = new AddCycleCountDocumentLineCommand(
            documentGuid,
            request.ItemId,
            request.LocationId,
            request.ExpectedQuantity,
            request.StockLotId,
            request.Notes,
            request.DocumentVersion);

        var result = await sender.Send(command, ct);

        return result.ToActionResult(
            this,
            v => new AddCycleCountDocumentLineResponse(v),
            nameof(CycleCountDocumentsController.GetDocument),
            "CycleCountDocuments",
            new { documentGuid });
    }

    /// <summary>
    /// Retrieves all lines associated with the specified cycle count document.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the cycle count document for which lines are being requested.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of
    /// <see cref="CycleCountDocumentLineDto"/> objects representing the lines of the specified cycle count document.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<IActionResult> GetLines(
        Guid documentGuid,
        CancellationToken ct)
    {
        var query = new GetCycleCountDocumentLinesQuery(documentGuid);
        var result = await sender.Send(query, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Updates an existing line in the cycle count document with new values.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the cycle count document that contains the line to be updated.</param>
    /// <param name="lineGuid">The unique identifier of the line to be updated within the cycle count document.</param>
    /// <param name="request">The request object containing the updated details for the line, including actual quantity,
    /// notes, and document version.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task completes when the line is successfully updated.</returns>
    [HttpPatch("{lineGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchLine(
        Guid documentGuid,
        Guid lineGuid,
        [FromBody] PatchCycleCountDocumentLineRequest request,
        CancellationToken ct)
    {
        var command = new PatchCycleCountDocumentLineCommand(
            documentGuid,
            lineGuid,
            request.ExpectedQuantity,
            request.Notes,
            request.DocumentVersion);
        var result = await sender.Send(command, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Deletes a line from the cycle count document.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the cycle count document from which the line is to be deleted.</param>
    /// <param name="lineGuid">The unique identifier of the line to be deleted.</param>
    /// <param name="documentVersion">The version of the document to ensure concurrency control during the operation.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. No content is returned upon successful completion.</returns>
    [HttpDelete("{lineGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLine(
        Guid documentGuid,
        Guid lineGuid,
        [FromQuery] long documentVersion,
        CancellationToken ct)
    {
        var command = new DeleteCycleCountDocumentLineCommand(
            documentGuid,
            lineGuid,
            documentVersion);
        var result = await sender.Send(command, ct);
        return result.ToActionResult(this);
    }
}
