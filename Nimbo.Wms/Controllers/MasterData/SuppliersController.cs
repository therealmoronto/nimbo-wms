using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts.MasterData.Commands;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Contracts.MasterData.Queries;
using Nimbo.Wms.Extensions;
using Nimbo.Wms.Models.MasterData;

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
        var command = new CreateSupplierCommand(request.Code, request.Name);
        var result = await sender.Send(command, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Retrieves a supplier, including its associated items, based on the supplier's unique identifier.
    /// </summary>
    /// <returns>A data transfer object representing the supplier and its associated items.</returns>
    [HttpGet("{supplierGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<IActionResult> GetSupplier([FromRoute] Guid supplierGuid, CancellationToken ct)
    {
        var query = new GetSupplierQuery(supplierGuid);
        var result = await sender.Send(query, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Retrieves a collection of all suppliers without including supplier items.
    /// </summary>
    /// <returns>A read-only list of <see cref="SupplierDto"/> representing the suppliers.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<IActionResult> GetSuppliers(CancellationToken ct)
    {
        var result = await sender.Send(new GetSuppliersQuery(), ct);
        return result.ToActionResult(this);
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
        var command = new PatchSupplierCommand(
            supplierGuid,
            request.Code,
            request.Name,
            request.TaxId,
            request.Address,
            request.ContactName,
            request.Phone,
            request.Email,
            request.IsActive);

        var result = await sender.Send(command, ct);
        return result.ToActionResult(this);
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
        var result = await sender.Send(new DeleteSupplierCommand(supplierGuid), ct);
        return result.ToActionResult(this);
    }
}
