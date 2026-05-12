using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts.Topology.Requests;
using Nimbo.Wms.Models.Topology;

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
        var command = new PatchLocationCommand(
            locationGuid,
            request.Code,
            request.Type,
            request.MaxWeightKg,
            request.MaxVolumeM3,
            request.IsSingleItemOnly,
            request.IsPickingLocation,
            request.IsReceivingLocation,
            request.IsShippingLocation,
            request.IsActive,
            request.IsBlocked,
            request.Aisle,
            request.Rack,
            request.Level,
            request.Position);

        await sender.Send(command, ct);
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
        await sender.Send(new DeleteLocationCommand(locationGuid), ct);
        return NoContent();
    }
}
