using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;
using PatchZoneRequest = Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands.PatchZoneRequest;

namespace Nimbo.Wms.Controllers.Topology;

[ApiController]
[Route("api/topology/zones")]
public class ZonesController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Updates the properties of a zone identified by the specified GUID.
    /// </summary>
    /// <returns>
    /// A task that represents the result of the asynchronous operation.
    /// Returns an HTTP 204 (No Content) status if the zone is successfully updated.
    /// Returns an HTTP 404 (Not Found) status if the zone does not exist.
    /// Returns an HTTP 400 (Bad Request) status if the input data is invalid.
    /// </returns>
    [HttpPatch("{zoneGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PatchZone(
        [FromRoute] Guid zoneGuid,
        [FromBody] PatchZoneRequest request,
        CancellationToken ct)
    {
        await sender.Send(request with { ZoneGuid = zoneGuid }, ct);
        return NoContent();
    }

    /// <summary>
    /// Deletes a zone identified by the provided GUID.
    /// </summary>
    /// <returns>
    /// A task that represents the result of the asynchronous operation.
    /// Returns an HTTP 204 (No Content) status if the zone is successfully deleted.
    /// Returns an HTTP 404 (Not Found) status if the zone does not exist.
    /// Returns an HTTP 400 (Bad Request) status if the input data is invalid.
    /// </returns>
    [HttpDelete("{zoneGuid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteZone(
        [FromRoute] Guid zoneGuid,
        CancellationToken ct)
    {
        await sender.Send(new DeleteZoneRequest(zoneGuid), ct);
        return NoContent();
    }
}
