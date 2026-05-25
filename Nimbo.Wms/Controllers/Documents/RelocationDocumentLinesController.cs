using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts.Documents.Relocation.Commands;
using Nimbo.Wms.Contracts.Documents.Relocation.Dtos;
using Nimbo.Wms.Contracts.Documents.Relocation.Queries;
using Nimbo.Wms.Models.Documents.Relocation;

namespace Nimbo.Wms.Controllers.Documents;

[ApiController]
[Route("api/documents/relocation/{documentGuid:guid}/lines")]
public class RelocationDocumentLinesController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Adds a new line to the relocation document.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the relocation document to which the line will be added.</param>
    /// <param name="request">The details of the line to be added, including item ID, origin and destination locations, quantity, notes, and the document version.</param>
    /// <param name="ct">The cancellation token to observe, which can be used to send a cancellation request to the operation.</param>
    /// <returns>Returns an HTTP 201 Created response containing the unique identifier of the newly created line.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Produces("application/json")]
    public async Task<IActionResult> AddLine(
        Guid documentGuid,
        [FromBody] AddRelocationDocumentLineRequest request,
        CancellationToken ct)
    {
        var command = new AddRelocationDocumentLineCommand(
            documentGuid,
            request.ItemId,
            request.FromLocationId,
            request.ToLocationId,
            request.Quantity,
            request.Notes,
            request.DocumentVersion);

        var lineGuid = await sender.Send(command, ct);
        return CreatedAtAction(
            nameof(GetLines),
            new { documentGuid, lineGuid },
            new AddRelocationDocumentLineResponse(lineGuid));
    }

    /// <summary>
    /// Retrieves all lines associated with the specified relocation document.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the relocation document from which lines are to be retrieved.</param>
    /// <param name="ct">The cancellation token to observe, which can be used to send a cancellation request to the operation.</param>
    /// <returns>Returns a list of relocation document lines, including details regarding item ID, origin and destination locations, quantity, and notes.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<IReadOnlyList<RelocationDocumentLineDto>> GetLines(
        Guid documentGuid,
        CancellationToken ct)
    {
        var query = new GetRelocationDocumentLinesQuery(documentGuid);
        return await sender.Send(query, ct);
    }

    /// <summary>
    /// Updates the specified line in the relocation document with the provided details.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the relocation document containing the line to be updated.</param>
    /// <param name="lineGuid">The unique identifier of the line to be updated within the relocation document.</param>
    /// <param name="request">The details to update the line, including optional origin and destination locations, quantity, and notes, along with the document version for concurrency control.</param>
    /// <param name="ct">The cancellation token to observe, which can be used to send a cancellation request to the operation.</param>
    /// <returns>Returns an HTTP 204 No Content response if the update is successful, or an HTTP 404 Not Found response if the line does not exist.</returns>
    [HttpPatch("{lineGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchLine(
        Guid documentGuid,
        Guid lineGuid,
        [FromBody] PatchRelocationDocumentLineRequest request,
        CancellationToken ct)
    {
        var command = new PatchRelocationDocumentLineCommand(
            documentGuid,
            lineGuid,
            request.FromLocationId,
            request.ToLocationId,
            request.Quantity,
            request.Notes,
            request.DocumentVersion);
        await sender.Send(command, ct);
        return NoContent();
    }

    /// <summary>
    /// Deletes a specific line from the relocation document.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the relocation document from which the line will be deleted.</param>
    /// <param name="lineGuid">The unique identifier of the line to be deleted.</param>
    /// <param name="documentVersion">The version of the relocation document to validate against for concurrency control.</param>
    /// <param name="ct">The cancellation token to observe, which can be used to send a cancellation request to the operation.</param>
    /// <returns>Returns an HTTP 204 No Content response if the deletion is successful; otherwise, it returns an HTTP 404 Not Found response if the line does not exist.</returns>
    [HttpDelete("{lineGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLine(
        Guid documentGuid,
        Guid lineGuid,
        [FromQuery] long documentVersion,
        CancellationToken ct)
    {
        var command = new DeleteRelocationDocumentLineCommand(
            documentGuid,
            lineGuid,
            documentVersion);
        await sender.Send(command, ct);
        return NoContent();
    }
}
