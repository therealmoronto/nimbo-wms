using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;
using Nimbo.Wms.Contracts.Topology.Http;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Controllers;

[ApiController]
[Route("api/topology/locations")]
public class LocationsController : ControllerBase
{
    /// <summary>
    /// Patches location.
    /// Returns 404 if warehouse does not exist.
    /// </summary>
    /// <response code="204">No content</response>
    /// <response code="404">Not found</response>
    /// <response code="400">Bad request</response>
    [HttpPatch("{locationGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PatchLocation(
        [FromRoute] Guid locationGuid,
        [FromBody] PatchLocationRequest request,
        [FromServices] ICommandHandler<PatchLocationCommand> handler,
        CancellationToken ct)
    {
        var locationId = LocationId.From(locationGuid);
        await handler.HandleAsync(new PatchLocationCommand(locationId, request), ct);
        return NoContent();
    }

    /// <summary>
    /// Deletes location.
    /// Returns 404 if warehouse does not exist.
    /// </summary>
    /// <response code="204">No content</response>
    /// <response code="404">Not found</response>
    [HttpDelete("{locationGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLocation(
        [FromRoute] Guid locationGuid,
        [FromServices] ICommandHandler<DeleteLocationCommand> handler,
        CancellationToken ct)
    {
        var locationId = LocationId.From(locationGuid);
        await handler.HandleAsync(new DeleteLocationCommand(locationId), ct);
        return NoContent();
    }
}
