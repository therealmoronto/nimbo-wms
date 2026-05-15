using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts.Documents.Receiving.Commands;
using Nimbo.Wms.Contracts.Documents.Receiving.Dtos;
using Nimbo.Wms.Contracts.Documents.Receiving.Queries;
using Nimbo.Wms.Models.Documents.Receiving;

namespace Nimbo.Wms.Controllers.Documents;

[ApiController]
[Route("api/documents/receiving/{documentGuid:guid}/lines")]
public class ReceivingDocumentLinesController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Adds a new line to the receiving document.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the receiving document to which the line is being added.</param>
    /// <param name="request">The request object containing the details of the line to be added, such as the location,
    /// received quantity, expected quantity, notes, and document version.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an
    /// <see cref="AddReceivingDocumentLineResponse"/> object with the unique identifier of the added line.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Produces("application/json")]
    public async Task<IActionResult> AddLine(
        Guid documentGuid,
        [FromBody] AddReceivingDocumentLineRequest request,
        CancellationToken ct)
    {
        var command = new AddReceivingDocumentLineCommand(
            documentGuid,
            request.ToLocationId,
            request.ReceivedQuantity,
            request.ExpectedQuantity,
            request.Notes,
            request.DocumentVersion);

        var lineGuid = await sender.Send(command, ct);
        return CreatedAtAction(
            nameof(GetLines),
            new { documentGuid, lineGuid },
            new AddReceivingDocumentLineResponse(lineGuid));
    }

    /// <summary>
    /// Retrieves all lines associated with the specified receiving document.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the receiving document for which lines are being requested.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of
    /// <see cref="ReceivingDocumentLineDto"/> objects representing the lines of the specified receiving document.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<IReadOnlyList<ReceivingDocumentLineDto>> GetLines(
        Guid documentGuid,
        CancellationToken ct)
    {
        var query = new GetReceivingDocumentLinesQuery(documentGuid);
        return await sender.Send(query, ct);
    }

    /// <summary>
    /// Updates an existing line in the receiving document with new values.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the receiving document that contains the line to be updated.</param>
    /// <param name="lineGuid">The unique identifier of the line to be updated within the receiving document.</param>
    /// <param name="request">The request object containing the updated details for the line, including location,
    /// received quantity, expected quantity, notes, and document version.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task completes when the line is successfully updated.</returns>
    [HttpPatch("{lineGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchLine(
        Guid documentGuid,
        Guid lineGuid,
        [FromBody] PatchReceivingDocumentLineRequest request,
        CancellationToken ct)
    {
        var command = new PatchReceivingDocumentLineCommand(
            documentGuid,
            lineGuid,
            request.ToLocationId,
            request.ReceivedQuantity,
            request.ExpectedQuantity,
            request.Notes,
            request.DocumentVersion);
        await sender.Send(command, ct);
        return NoContent();
    }

    /// <summary>
    /// Deletes a line from the receiving document.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the receiving document from which the line is to be deleted.</param>
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
        var command = new DeleteReceivingDocumentLineCommand(
            documentGuid,
            lineGuid,
            documentVersion);
        await sender.Send(command, ct);
        return NoContent();
    }
}
