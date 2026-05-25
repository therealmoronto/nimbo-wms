using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts.Documents.Shipment.Commands;
using Nimbo.Wms.Contracts.Documents.Shipment.Dtos;
using Nimbo.Wms.Contracts.Documents.Shipment.Queries;
using Nimbo.Wms.Models.Documents.Shipment;

namespace Nimbo.Wms.Controllers.Documents;

[ApiController]
[Route("api/documents/shipment")]
public class ShipmentDocumentsController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Creates a new shipment document and returns a response containing its unique identifier.
    /// </summary>
    /// <param name="request">The details of the shipment document to create, including warehouse, code, and title.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="IActionResult"/> containing the unique identifier of the newly created shipment document
    /// and a status code of Created if successful.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Produces("application/json")]
    public async Task<IActionResult> CreateDocument(
        [FromBody] CreateShipmentDocumentRequest request,
        CancellationToken ct)
    {
        var command = new CreateShipmentDocumentCommand(
            request.WarehouseId,
            request.Code,
            request.Title);
        var documentGuid = await sender.Send(command, ct);

        return CreatedAtAction(
            actionName: nameof(GetDocument),
            new { documentGuid = documentGuid },
            new CreateShipmentDocumentResponse(documentGuid));
    }

    /// <summary>
    /// Retrieves a list of all shipment documents.
    /// </summary>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="IActionResult"/> containing a list of shipment documents and a status code of OK if successful.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<IReadOnlyList<ShipmentDocumentBodyDto>> GetDocuments(CancellationToken ct)
    {
        var query = new GetShipmentDocumentsQuery();
        return await sender.Send(query, ct);
    }

    /// <summary>
    /// Retrieves a shipment document by its unique identifier.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the shipment document to retrieve.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="ShipmentDocumentDto"/> containing the details of the shipment document if found,
    /// or a suitable status code otherwise.</returns>
    [HttpGet("{documentGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<ShipmentDocumentDto> GetDocument(Guid documentGuid, CancellationToken ct)
    {
        var query = new GetShipmentDocumentQuery(documentGuid);
        return await sender.Send(query, ct);
    }

    /// <summary>
    /// Updates an existing shipment document with new values for code, title, or notes.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the shipment document to update.</param>
    /// <param name="request">The details of the updates to apply to the shipment document, including optional changes
    /// to code, title, notes, and the document version for concurrency control.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="IActionResult"/> with a status code of NoContent if the update is successful.</returns>
    [HttpPatch("{documentGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchDocument(
        Guid documentGuid,
        [FromBody] PatchShipmentDocumentRequest request,
        CancellationToken ct)
    {
        var command = new PatchShipmentDocumentCommand(
            documentGuid,
            request.CustomerId,
            request.Code,
            request.Title,
            request.Notes,
            request.Version);
        await sender.Send(command, ct);
        return NoContent();
    }

    /// <summary>
    /// Deletes a shipment document by its unique identifier.
    /// </summary>
    /// <param name="documentGuid">The unique identifier of the shipment document to delete.</param>
    /// <param name="version">The version of the document for concurrency control.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="IActionResult"/> with a status code of NoContent if the deletion is successful.</returns>
    [HttpDelete("{documentGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDocument(
        Guid documentGuid,
        [FromQuery] long version,
        CancellationToken ct)
    {
        var command = new DeleteShipmentDocumentCommand(documentGuid, version);
        await sender.Send(command, ct);
        return NoContent();
    }
}
