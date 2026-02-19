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
    /// Updates the attributes of a location identified by its unique identifier.
    /// </summary>
    /// <returns>An asynchronous operation that returns a NoContent response if the update is successful.
    /// Returns a NotFound response if the location does not exist. Returns a BadRequest response if the input data is invalid.</returns>
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
    /// Deletes a location identified by its unique identifier.
    /// </summary>
    /// <returns>An asynchronous operation that returns a NoContent response if the deletion is successful.
    /// Returns a NotFound response if the location does not exist.</returns>
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
