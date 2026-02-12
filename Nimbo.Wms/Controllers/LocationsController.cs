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
}
