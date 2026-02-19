using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Commands;
using Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Queries;
using Nimbo.Wms.Contracts.MasterData.Http;
using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Contracts.Topology.Http;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Controllers;

[ApiController]
[Route("api/suppliers")]
public class SuppliersController : ControllerBase
{
    /// <summary>
    /// Create a new supplier.
    /// </summary>
    /// <returns>An action result containing the response of the created supplier, including its identifier.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> CreateSupplier(
        [FromBody] CreateSupplierRequest request,
        [FromServices] ICommandHandler<CreateSupplierCommand, SupplierId> handler,
        CancellationToken ct)
    {
        var command = new CreateSupplierCommand(request);
        var supplierId = await handler.HandleAsync(command, ct);
        
        return CreatedAtAction(
            actionName: nameof(GetSupplier),
            controllerName: "Suppliers",
            routeValues: new { supplierGuid = supplierId.Value },
            value: new CreateSupplierResponse(supplierId.Value));
    }

    /// <summary>
    /// Retrieves a supplier, including its associated items, based on the supplier's unique identifier.
    /// </summary>
    /// <returns>A data transfer object representing the supplier and its associated items.</returns>
    [HttpGet("{supplierGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<SupplierDto> GetSupplier(
        [FromRoute] Guid supplierGuid,
        [FromServices] IQueryHandler<GetSupplierQuery, SupplierDto> handler,
        CancellationToken ct)
    {
        var supplierId = SupplierId.From(supplierGuid);
        var query = new GetSupplierQuery(supplierId);
        return await handler.HandleAsync(query, ct);
    }

    /// <summary>
    /// Retrieves a collection of all suppliers without including supplier items.
    /// </summary>
    /// <returns>A read-only list of <see cref="SupplierDto"/> representing the suppliers.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<IReadOnlyList<SupplierDto>> GetSuppliers(
        [FromServices] IQueryHandler<GetSuppliersQuery, IReadOnlyList<SupplierDto>> handler,
        CancellationToken ct)
    {
        return await handler.HandleAsync(new GetSuppliersQuery(), ct);
    }

    /// <summary>
    /// Applies a partial update to an existing supplier.
    /// </summary>
    /// <returns>An action result indicating the outcome of the operation.</returns>
    [HttpPatch("{supplierGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PatchSupplier(
        [FromRoute] Guid supplierGuid,
        [FromBody] PatchSupplierRequest request,
        [FromServices] ICommandHandler<PatchSupplierCommand> handler,
        CancellationToken ct)
    {
        var supplierId = SupplierId.From(supplierGuid);
        var command = new PatchSupplierCommand(supplierId, request);
        await handler.HandleAsync(command, ct);
        return NoContent();
    }

    /// <summary>
    /// Deletes an existing supplier identified by the specified GUID.
    /// </summary>
    /// <returns>An action result indicating the outcome of the delete operation.</returns>
    [HttpDelete("{supplierGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSupplier(
        [FromRoute] Guid supplierGuid,
        [FromServices] ICommandHandler<DeleteSupplierCommand> handler,
        CancellationToken ct)
    {
        var supplierId = SupplierId.From(supplierGuid);
        await handler.HandleAsync(new DeleteSupplierCommand(supplierId), ct);
        return NoContent();
    }
}
