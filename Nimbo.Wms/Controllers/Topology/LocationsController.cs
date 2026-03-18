using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts.Topology.Requests;
using Nimbo.Wms.Domain.Identification;
using PatchLocationRequest = Nimbo.Wms.Contracts.Topology.Requests.PatchLocationRequest;

namespace Nimbo.Wms.Controllers.Topology;

[ApiController]
[Route("api/topology/locations")]
public class LocationsController(ISender sender) : ControllerBase
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
    public async Task<IActionResult> PatchLocation([FromRoute] Guid locationGuid, [FromBody] PatchLocationRequest request, CancellationToken ct)
    {
        await sender.Send(request with { LocationGuid = locationGuid }, ct);
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
    public async Task<IActionResult> DeleteLocation([FromRoute] Guid locationGuid, CancellationToken ct)
    {
        var locationId = LocationId.From(locationGuid);
        await sender.Send(new DeleteLocationRequest(locationId), ct);
        return NoContent();
    }
}
