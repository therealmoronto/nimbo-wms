using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts.Topology.Commands;
using Nimbo.Wms.Extensions;
using Nimbo.Wms.Models.Topology;

namespace Nimbo.Wms.Controllers.Topology;

[ApiController]
[Route("api/topology/warehouses/{warehouseGuid:guid}")]
public class WarehouseTopologyController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Adds a new zone to the specified warehouse using the provided request data and command handler.
    /// </summary>
    /// <returns>
    /// An ActionResult containing a response with the identifier of the newly created zone.
    /// </returns>
    [HttpPost("zones")]
    [ProducesResponseType(typeof(AddZoneResponse), StatusCodes.Status201Created)]
    [Produces("application/json")]
    public async Task<IActionResult> AddZone(
        [FromRoute] Guid warehouseGuid,
        [FromBody] AddZoneRequest request,
        CancellationToken ct)
    {
        var command  = new AddZoneCommand(warehouseGuid, request.Code, request.Name, request.Type);
        var result = await sender.Send(command, ct);

        // Location header points to warehouse topology
        return result.ToActionResult(
            this,
            v => new AddZoneResponse(v),
            nameof(WarehousesController.GetWarehouseTopology),
            "Warehouses",
            new { warehouseGuid });
    }

    /// <summary>
    /// Adds a new location to the specified warehouse based on the provided request parameters.
    /// </summary>
    /// <returns>
    /// An ActionResult containing the response with the newly created location's identifier.
    /// </returns>
    [HttpPost("locations")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Produces("application/json")]
    public async Task<IActionResult> AddLocation(
        [FromRoute] Guid warehouseGuid,
        [FromBody] AddLocationRequest request,
        CancellationToken ct)
    {
        var command = new AddLocationCommand(warehouseGuid, request.ZoneGuid, request.Code, request.Type);
        var result = await sender.Send(command, ct);

        return result.ToActionResult(
            this,
            v => new AddLocationResponse(v),
            actionName: nameof(WarehousesController.GetWarehouseTopology),
            controllerName: "Warehouses",
            routeValues: new { warehouseGuid });
    }
}
