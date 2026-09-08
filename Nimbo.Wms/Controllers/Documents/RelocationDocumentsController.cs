using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts.Documents.Relocation.Commands;
using Nimbo.Wms.Contracts.Documents.Relocation.Dtos;
using Nimbo.Wms.Contracts.Documents.Relocation.Queries;
using Nimbo.Wms.Extensions;
using Nimbo.Wms.Models.Documents.Relocation;

namespace Nimbo.Wms.Controllers.Documents;

[ApiController]
[Route("api/documents/relocation")]
public class RelocationDocumentsController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Creates a new relocation document based on the provided request data.
    /// </summary>
    /// <param name="request">The details of the relocation document to create, including warehouse ID, code, title, and optional notes.</param>
    /// <param name="ct">The cancellation token used to propagate notifications that operations should be canceled.</param>
    /// <returns>Returns a 201 Created response with the unique identifier of the newly created relocation document.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Produces("application/json")]
    public async Task<IActionResult> CreateDocument(
        [FromBody] CreateRelocationDocumentRequest request,
        CancellationToken ct)
    {
        var command = new CreateRelocationDocumentCommand(
            request.WarehouseId,
            request.Code,
            request.Title);
        var result = await sender.Send(command, ct);

        return result.ToActionResult(
            this,
            v => new CreateRelocationDocumentResponse(v),
            nameof(GetDocument),
            "RelocationDocuments",
            result.IsSuccess ? new { documentGuid = result.Value } : null);
    }

    /// <summary>
    /// Retrieves a list of all relocation documents.
    /// </summary>
    /// <param name="ct">The cancellation token used to propagate notifications that operations should be canceled.</param>
    /// <returns>Returns a collection of relocation documents, including details such as ID, warehouse ID, code, title, status, timestamps, version, and optional notes.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<IActionResult> GetDocuments(CancellationToken ct)
    {
        var query = new GetRelocationDocumentsQuery();
        var result = await sender.Send(query, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Retrieves a specific relocation document by its unique identifier.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the relocation document to retrieve.</param>
    /// <param name="ct">The cancellation token used to propagate notifications that operations should be canceled.</param>
    /// <returns>Returns the details of the specified relocation document, including
    [HttpGet("{documentGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<IActionResult> GetDocument(Guid documentGuid, CancellationToken ct)
    {
        var query = new GetRelocationDocumentQuery(documentGuid);
        var result = await sender.Send(query, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Updates an existing relocation document with the specified changes.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the relocation document to update.</param>
    /// <param name="request">The details of the changes to apply to the relocation document, including optional updates to code, title, notes, and the expected version for concurrency control.</param>
    /// <param name="ct">The cancellation token used to propagate notifications that operations should be canceled.</param>
    /// <returns>Returns a 204 No Content response if the update is successful or a 404 Not Found response if the document does not exist.</returns>
    [HttpPatch("{documentGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchDocument(
        Guid documentGuid,
        [FromBody] PatchRelocationDocumentRequest request,
        CancellationToken ct)
    {
        var command = new PatchRelocationDocumentCommand(
            documentGuid,
            request.Code,
            request.Title,
            request.Notes,
            request.Version);
        var result = await sender.Send(command, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Deletes an existing relocation document based on the provided document identifier and version.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the relocation document to delete.</param>
    /// <param name="version">The version of the relocation document to ensure optimistic concurrency control.</param>
    /// <param name="ct">The cancellation token used to propagate notifications that operations should be canceled.</param>
    /// <returns>Returns a 204 No Content response if the deletion is successful, or a 404 Not Found response if the document does not exist.</returns>
    [HttpDelete("{documentGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDocument(Guid documentGuid, [FromQuery] long version, CancellationToken ct)
    {
        var command = new DeleteRelocationDocumentCommand(documentGuid, version);
        var result = await sender.Send(command, ct);
        return result.ToActionResult(this);
    }
}
