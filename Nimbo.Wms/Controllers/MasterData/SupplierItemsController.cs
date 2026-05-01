using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts.MasterData.Requests;

namespace Nimbo.Wms.Controllers.MasterData;

[ApiController]
[Route("api/suppliers/{supplierGuid:guid}/items")]
public class SupplierItemsController(ISender sender) : ControllerBase
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
        CancellationToken ct)
    {
        var supplierItemGuid = await sender.Send(request with { SupplierGuid = supplierGuid }, ct);

        return CreatedAtAction(
            actionName: nameof(SuppliersController.GetSupplier),
            controllerName: "Suppliers",
            routeValues: new { supplierGuid = supplierItemGuid },
            value: new AddSupplierItemResponse(supplierItemGuid));
    }

    /// <summary>
    /// Updates the details of a specific supplier item associated with a supplier.
    /// </summary>
    /// <returns>
    /// An <see cref="IActionResult"/> representing the result of the operation.
    /// Returns a 204 No Content status upon successful update. If the specified supplier or supplier item
    /// is not found, it may return a 404 Not Found status. For invalid inputs, it may return a 400 Bad Request status.
    /// </returns>
    [HttpPatch("{supplierItemGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PatchSupplierItem(
        [FromRoute] Guid supplierGuid,
        [FromRoute] Guid supplierItemGuid,
        [FromBody] PatchSupplierItemRequest request,
        CancellationToken ct)
    {
        await sender.Send(request with { SupplierGuid = supplierGuid, SupplierItemGuid = supplierItemGuid }, ct);
        return NoContent();
    }

    /// <summary>
    /// Deletes a specific supplier item associated with the specified supplier.
    /// </summary>
    /// <returns>
    /// An <see cref="IActionResult"/> representing the result of the operation. If successful, it
    /// returns a 204 No Content status. If the specified supplier or supplier item is not found,
    /// it returns a 404 Not Found status.
    /// </returns>
    [HttpDelete("{supplierItemGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSupplierItem(
        [FromRoute] Guid supplierGuid,
        [FromRoute] Guid supplierItemGuid,
        CancellationToken ct)
    {
        await sender.Send(new DeleteSupplierItemRequest(supplierGuid, supplierItemGuid), ct);
        return NoContent();
    }
}
