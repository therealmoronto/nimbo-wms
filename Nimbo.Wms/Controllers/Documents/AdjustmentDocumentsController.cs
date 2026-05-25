using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts.Documents.Adjustment.Commands;
using Nimbo.Wms.Contracts.Documents.Adjustment.Dtos;
using Nimbo.Wms.Contracts.Documents.Adjustment.Queries;
using Nimbo.Wms.Models.Documents.Adjustment;

namespace Nimbo.Wms.Controllers.Documents;

[ApiController]
[Route("api/documents/adjustment")]
public class AdjustmentDocumentsController(ISender sender) : ControllerBase
{
    /// Creates a new adjustment document and returns its identifier.
    /// <param name="request">
    /// The request object containing details necessary to create an adjustment document.
    /// Includes properties such as WarehouseId, Code, Title, ReasonCode, and optional ReasonText.
    /// </param>
    /// <param name="ct">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>
    /// An IActionResult containing the status of the operation. If successful, returns a 201 Created response
    /// with a CreateAdjustmentDocumentResponse containing the identifier of the newly created document.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Produces("application/json")]
    public async Task<IActionResult> CreateDocument(
        [FromBody] CreateAdjustmentDocumentRequest request,
        CancellationToken ct)
    {
        var command = new CreateAdjustmentDocumentCommand(
            request.WarehouseId,
            request.Code,
            request.Title,
            request.ReasonCode,
            request.ReasonText);
        var documentGuid = await sender.Send(command, ct);

        return CreatedAtAction(
            actionName: nameof(GetDocument),
            new { documentGuid = documentGuid },
            new CreateAdjustmentDocumentResponse(documentGuid));
    }

    /// Retrieves a list of all adjustment documents in the system.
    /// <param name="ct">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>
    /// A read-only list of AdjustmentDocumentBodyDto objects representing the adjustment documents.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<IReadOnlyList<AdjustmentDocumentBodyDto>> GetDocuments(CancellationToken ct)
    {
        var query = new GetAdjustmentDocumentsQuery();
        return await sender.Send(query, ct);
    }

    /// Retrieves an existing adjustment document based on its unique identifier.
    /// <param name="documentGuid">
    /// The unique identifier of the adjustment document to be retrieved.
    /// </param>
    /// <param name="ct">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>
    /// An AdjustmentDocumentDto containing the details of the requested adjustment document if found.
    /// If the document is not found, returns a 404 Not Found response.
    /// </returns>
    [HttpGet("{documentGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<AdjustmentDocumentDto> GetDocument(Guid documentGuid, CancellationToken ct)
    {
        var query = new GetAdjustmentDocumentQuery(documentGuid);
        return await sender.Send(query, ct);
    }

    /// Updates an existing adjustment document with new details.
    /// <param name="documentGuid">
    /// The unique identifier of the adjustment document to be updated.
    /// </param>
    /// <param name="request">
    /// The request object containing the new details for the adjustment document.
    /// Includes properties such as Code, Title, ReasonCode, ReasonText, and the Version of the document to prevent concurrency issues.
    /// </param>
    /// <param name="ct">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>
    /// An IActionResult indicating the result of the update operation. If the update is successful, a 204 No Content response is returned.
    /// If the document is not found, a 404 Not Found response is returned.
    /// </returns>
    [HttpPatch("{documentGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchDocument(
        Guid documentGuid,
        [FromBody] PatchAdjustmentDocumentRequest request,
        CancellationToken ct)
    {
        var command = new PatchAdjustmentDocumentCommand(
            documentGuid,
            request.Code,
            request.Title,
            request.ReasonCode,
            request.ReasonText,
            request.Version);
        await sender.Send(command, ct);
        return NoContent();
    }

    /// Deletes an existing adjustment document based on the provided identifier and version.
    /// <param name="documentGuid">
    /// The unique identifier of the adjustment document to be deleted.
    /// </param>
    /// <param name="version">
    /// The specific version of the adjustment document to ensure concurrency handling during deletion.
    /// </param>
    /// <param name="ct">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>
    /// An IActionResult indicating the result of the operation. Returns a 204 No Content response if the deletion is successful,
    /// or a 404 Not Found response if the document does not exist.
    /// </returns>
    [HttpDelete("{documentGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDocument(Guid documentGuid, [FromQuery] long version, CancellationToken ct)
    {
        var command = new DeleteAdjustmentDocumentCommand(documentGuid, version);
        await sender.Send(command, ct);
        return NoContent();
    }
}
