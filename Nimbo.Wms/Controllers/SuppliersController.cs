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
    /// Create supplier.
    /// </summary>
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
    /// Get supplier including supplier items.
    /// </summary>
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
    /// Get all suppliers flat list excluding supplier items.
    /// </summary>
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
    /// Patch supplier with partial update.
    /// </summary>
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
