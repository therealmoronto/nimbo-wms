using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts.Documents.CycleCount.Commands;
using Nimbo.Wms.Contracts.Documents.CycleCount.Dtos;
using Nimbo.Wms.Contracts.Documents.CycleCount.Queries;
using Nimbo.Wms.Extensions;
using Nimbo.Wms.Models.Documents.CycleCount;

namespace Nimbo.Wms.Controllers.Documents;

[ApiController]
[Route("api/documents/cycle-count")]
public class CycleCountDocumentsController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Creates a new cycle count document and returns a response containing its unique identifier.
    /// </summary>
    /// <param name="request">The details of the cycle count document to create, including warehouse, code, and title.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="IActionResult"/> containing the unique identifier of the newly created cycle count document
    /// and a status code of Created if successful.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Produces("application/json")]
    public async Task<IActionResult> CreateDocument(
        [FromBody] CreateCycleCountDocumentRequest request,
        CancellationToken ct)
    {
        var command = new CreateCycleCountDocumentCommand(
            request.WarehouseId,
            request.Code,
            request.Title);
        var result = await sender.Send(command, ct);

        return result.ToActionResult(
            this,
            v => new CreateCycleCountDocumentResponse(v),
            nameof(GetDocument),
            "CycleCountDocuments",
            result.IsSuccess ? new { documentGuid = result.Value } : null);
    }

    /// <summary>
    /// Retrieves a list of all cycle count documents.
    /// </summary>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="IActionResult"/> containing a list of cycle count documents and a status code of OK if successful.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<IActionResult> GetDocuments(CancellationToken ct)
    {
        var query = new GetCycleCountDocumentsQuery();
        var result = await sender.Send(query, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Retrieves a cycle count document by its unique identifier.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the cycle count document to retrieve.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="CycleCountDocumentDto"/> containing the details of the cycle count document if found,
    /// or a suitable status code otherwise.</returns>
    [HttpGet("{documentGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<IActionResult> GetDocument(Guid documentGuid, CancellationToken ct)
    {
        var query = new GetCycleCountDocumentQuery(documentGuid);
        var result = await sender.Send(query, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Updates an existing cycle count document with new values for code or title.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the cycle count document to update.</param>
    /// <param name="request">The details of the updates to apply to the cycle count document, including optional changes
    /// to code, title, and the document version for concurrency control.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="IActionResult"/> with a status code of NoContent if the update is successful.</returns>
    [HttpPatch("{documentGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchDocument(
        Guid documentGuid,
        [FromBody] PatchCycleCountDocumentRequest request,
        CancellationToken ct)
    {
        var command = new PatchCycleCountDocumentCommand(
            documentGuid,
            request.Code,
            request.Title,
            request.Version);
        var result = await sender.Send(command, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Deletes an existing cycle count document by its unique identifier and version.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the cycle count document to delete.</param>
    /// <param name="version">The version of the document to ensure it is the latest version being deleted.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="IActionResult"/> with a status code of NoContent if the document is deleted successfully,
    /// or NotFound if the document does not exist.</returns>
    [HttpDelete("{documentGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDocument(Guid documentGuid, [FromQuery] long version, CancellationToken ct)
    {
        var command = new DeleteCycleCountDocumentCommand(documentGuid, version);
        var result = await sender.Send(command, ct);
        return result.ToActionResult(this);
    }
}
