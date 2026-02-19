using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;
using Nimbo.Wms.Contracts.Topology.Http;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Controllers;

[ApiController]
[Route("api/topology/zones")]
public class ZonesController : ControllerBase
{
    /// <summary>
    /// Patches zone.
    /// Returns 404 if warehouse does not exist.
    /// </summary>
    /// <response code="204">No content</response>
    /// <response code="404">Not found</response>
    /// <response code="400">Bad request</response>
    [HttpPatch("{zoneGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PatchZone(
        [FromRoute] Guid zoneGuid,
        [FromBody] PatchZoneRequest request,
        [FromServices] ICommandHandler<PatchZoneCommand> handler,
        CancellationToken ct)
    {
        var zoneId = ZoneId.From(zoneGuid);
        await handler.HandleAsync(new PatchZoneCommand(zoneId, request), ct);
        return NoContent();
    }

    /// <summary>
    /// Deletes zone.
    /// Returns 404 if warehouse does not exist.
    /// </summary>
    /// <response code="204">No content</response>
    /// <response code="404">Not found</response>
    [HttpDelete("{zoneGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteZone(
        [FromRoute] Guid zoneGuid,
        [FromServices] ICommandHandler<DeleteZoneCommand> handler,
        CancellationToken ct)
    {
        var zoneId = ZoneId.From(zoneGuid);
        await handler.HandleAsync(new DeleteZoneCommand(zoneId), ct);
        return NoContent();
    }
}
