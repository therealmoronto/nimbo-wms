using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.UseCases.Stock.Commands;
using Nimbo.Wms.Application.Abstractions.UseCases.Stock.Queries;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Contracts.Stock.Http;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Controllers;

[ApiController]
[Route("api/stock")]
public class BatchesController : ControllerBase
{
    /// <summary>
    /// Creates a new batch in the system.
    /// </summary>
    /// <returns>
    /// Returns an IActionResult indicating the result of the batch creation process.
    /// If successful, returns a 201 Created status with the created batch details.
    /// If the request is invalid, returns a 400 Bad Request status.
    /// </returns>
    [HttpPost("batches")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> CreateBatch(
        [FromBody] CreateBatchRequest request,
        [FromServices] ICommandHandler<CreateBatchCommand, BatchId> handler,
        CancellationToken ct)
    {
        var command = new CreateBatchCommand(request);
        var batchId = await handler.HandleAsync(command, ct);
        return CreatedAtAction(
            nameof(GetBatch),
            "Batches",
            new { batchGuid = batchId.Value },
            new CreateBatchResponse(batchId));
    }

    /// <summary>
    /// Retrieves a batch by its unique identifier.
    /// </summary>
    /// <returns>
    /// Returns a <see cref="BatchDto"/> containing the details of the batch if found.
    /// If the batch is not found, an appropriate HTTP response status is returned.
    /// </returns>
    [HttpGet("batches/{batchGuid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<BatchDto> GetBatch(
        [FromRoute] Guid batchGuid,
        [FromServices] IQueryHandler<GetBatchQuery, BatchDto> handler,
        CancellationToken ct)
    {
        var query = new GetBatchQuery(BatchId.From(batchGuid));
        return await handler.HandleAsync(query, ct);
    }

    /// <summary>
    /// Retrieves a list of batches based on the provided filter criteria.
    /// </summary>
    /// <param name="request">
    /// The request containing filtering options such as ItemId and SupplierId.
    /// If these values are provided, the results will be filtered accordingly.
    /// </param>
    /// <param name="handler">
    /// The query handler responsible for processing the request and retrieving the matching batches.
    /// </param>
    /// <param name="ct">
    /// A cancellation token that can be used to cancel the operation.
    /// </param>
    /// <returns>
    /// Returns an IReadOnlyList of BatchDto representing the batches that match the given criteria.
    /// If no filter criteria are provided, all batches are returned.
    /// </returns>
    [HttpGet("batches")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<IReadOnlyList<BatchDto>> GetBatches(
        [FromBody] GetBatchesRequest request,
        [FromServices] IQueryHandler<GetBatchesQuery, IReadOnlyList<BatchDto>> handler,
        CancellationToken ct)
    {
        var itemId = request.ItemId.HasValue ? ItemId.From(request.ItemId.Value) : (ItemId?)null;
        var supplierId = request.SupplierId.HasValue ? SupplierId.From(request.SupplierId.Value) : (SupplierId?)null;
        var query = new GetBatchesQuery(itemId, supplierId);
        return await handler.HandleAsync(query, ct);
    }
}
