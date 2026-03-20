using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Contracts.Topology.Dtos;

namespace Nimbo.Wms.Controllers.MasterData;

[ApiController]
[Route("api/suppliers")]
public class SuppliersController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Create a new supplier.
    /// </summary>
    /// <returns>An action result containing the response of the created supplier, including its identifier.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierRequest request, CancellationToken ct)
    {
        var supplierId = await sender.Send(request, ct);
        
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
    public async Task<SupplierDto> GetSupplier([FromRoute] Guid supplierGuid, CancellationToken ct)
    {
        var request = new GetSupplierRequest(supplierGuid);
        return await sender.Send(request, ct);
    }

    /// <summary>
    /// Retrieves a collection of all suppliers without including supplier items.
    /// </summary>
    /// <returns>A read-only list of <see cref="SupplierDto"/> representing the suppliers.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<IReadOnlyList<SupplierDto>> GetSuppliers(CancellationToken ct)
    {
        return await sender.Send(new GetSuppliersRequest(), ct);
    }

    /// <summary>
    /// Applies a partial update to an existing supplier.
    /// </summary>
    /// <returns>An action result indicating the outcome of the operation.</returns>
    [HttpPatch("{supplierGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PatchSupplier([FromRoute] Guid supplierGuid, [FromBody] PatchSupplierRequest request, CancellationToken ct)
    {
        await sender.Send(request with { SupplierId = supplierGuid }, ct);
        return NoContent();
    }

    /// <summary>
    /// Deletes an existing supplier identified by the specified GUID.
    /// </summary>
    /// <returns>An action result indicating the outcome of the delete operation.</returns>
    [HttpDelete("{supplierGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSupplier([FromRoute] Guid supplierGuid, CancellationToken ct)
    {
        await sender.Send(new DeleteSupplierRequest(supplierGuid), ct);
        return NoContent();
    }
}
