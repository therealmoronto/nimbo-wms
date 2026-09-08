using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts.Documents.Receiving.Commands;
using Nimbo.Wms.Contracts.Documents.Receiving.Dtos;
using Nimbo.Wms.Contracts.Documents.Receiving.Queries;
using Nimbo.Wms.Extensions;
using Nimbo.Wms.Models.Documents.Receiving;

namespace Nimbo.Wms.Controllers.Documents;

[ApiController]
[Route("api/documents/receiving")]
public class ReceivingDocumentsController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Creates a new receiving document and returns a response containing its unique identifier.
    /// </summary>
    /// <param name="request">The details of the receiving document to create, including warehouse, supplier, code, title, and optional notes.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="IActionResult"/> containing the unique identifier of the newly created receiving document
    /// and a status code of Created if successful.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Produces("application/json")]
    public async Task<IActionResult> CreateDocument(
        [FromBody] CreateReceivingDocumentRequest request,
        CancellationToken ct)
    {
        var command = new CreateReceivingDocumentCommand(
            request.WarehouseId,
            request.SupplierId,
            request.Code,
            request.Title,
            request.Notes);
        var result = await sender.Send(command, ct);

        return result.ToActionResult(
            this,
            v => new CreateReceivingDocumentResponse(v),
            nameof(GetDocument),
            "ReceivingDocuments",
            result.IsSuccess ? new { documentGuid = result.Value } : null);
    }

    /// <summary>
    /// Retrieves a list of all receiving documents.
    /// </summary>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="IActionResult"/> containing a list of receiving documents and a status code of OK if successful.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<IActionResult> GetDocuments(CancellationToken ct)
    {
        var query = new GetReceivingDocumentsQuery();
        var result = await sender.Send(query, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Retrieves a receiving document by its unique identifier.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the receiving document to retrieve.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="ReceivingDocumentDto"/> containing the details of the receiving document if found,
    /// or a suitable status code otherwise.</returns>
    [HttpGet("{documentGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<IActionResult> GetDocument(Guid documentGuid, CancellationToken ct)
    {
        var query = new GetReceivingDocumentQuery(documentGuid);
        var result = await sender.Send(query, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Updates an existing receiving document with new values for supplier, code, title, or notes.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the receiving document to update.</param>
    /// <param name="request">The details of the updates to apply to the receiving document, including optional changes
    /// to supplier, code, title, notes, and the document version for concurrency control.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="IActionResult"/> with a status code of NoContent if the update is successful.</returns>
    [HttpPatch("{documentGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchDocument(
        Guid documentGuid,
        [FromBody] PatchReceivingDocumentRequest request,
        CancellationToken ct)
    {
        var command = new PatchReceivingDocumentCommand(
            documentGuid,
            request.Code,
            request.Title,
            request.Notes,
            request.Version);
        var result = await sender.Send(command, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Deletes an existing receiving document by its unique identifier and version.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the receiving document to delete.</param>
    /// <param name="version">The version of the document to ensure it is the latest version being deleted.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="IActionResult"/> with a status code of NoContent if the document is deleted successfully,
    /// or NotFound if the document does not exist.</returns>
    [HttpDelete("{documentGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDocument(Guid documentGuid, [FromQuery] long version, CancellationToken ct)
    {
        var command = new DeleteReceivingDocumentCommand(documentGuid, version);
        var result = await sender.Send(command, ct);
        return result.ToActionResult(this);
    }
}
