using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Commands;
using Nimbo.Wms.Contracts.MasterData.Http;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Controllers;

[ApiController]
[Route("api/suppliers/{supplierGuid:guid}/items")]
public class SupplierItemsController : ControllerBase
{
    /// <summary>
    /// Adds a new supplier item to the specified supplier.
    /// </summary>
    /// <returns>
    /// An <see cref="IActionResult"/> representing the result of the operation. If successful, it
    /// returns a 201 Created status with the newly created supplier item's details; otherwise,
    /// it may return an error status such as 400 Bad Request or 404 Not Found.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> AddSupplierItem(
        [FromRoute] Guid supplierGuid,
        [FromBody] AddSupplierItemRequest request,
        [FromServices] ICommandHandler<AddSupplierItemCommand, SupplierItemId> handler,
        CancellationToken ct)
    {
        var supplierId = SupplierId.From(supplierGuid);
        var command = new AddSupplierItemCommand(request with { SupplierId = supplierId });
        var supplierItemId = await handler.HandleAsync(command, ct);

        return CreatedAtAction(
            actionName: nameof(SuppliersController.GetSupplier),
            controllerName: "Suppliers",
            routeValues: new { supplierGuid },
            value: new AddSupplierItemResponse(supplierItemId.Value));
    }

    [HttpPatch("{supplierItemGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PatchSupplierItem(
        [FromRoute] Guid supplierGuid,
        [FromRoute] Guid supplierItemGuid,
        [FromBody] PatchSupplierItemRequest request,
        [FromServices] ICommandHandler<PatchSupplierItemCommand> handler,
        CancellationToken ct)
    {
        var supplierId = SupplierId.From(supplierGuid);
        var supplierItemId = SupplierItemId.From(supplierItemGuid);
        await handler.HandleAsync(new PatchSupplierItemCommand(supplierId, supplierItemId, request), ct);
        return NoContent();
    }

    [HttpDelete("{supplierItemGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSupplier(
        [FromRoute] Guid supplierGuid,
        [FromRoute] Guid supplierItemGuid,
        [FromServices] ICommandHandler<DeleteSupplierItemCommand> handler,
        CancellationToken ct)
    {
        var supplierId = SupplierId.From(supplierGuid);
        var supplierItemId = SupplierItemId.From(supplierItemGuid);
        await handler.HandleAsync(new DeleteSupplierItemCommand(supplierId, supplierItemId), ct);
        return NoContent();
    }
}
