using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts.Topology.Requests;
using AddLocationRequest = Nimbo.Wms.Models.Topology.AddLocationRequest;
using AddLocationResponse = Nimbo.Wms.Models.Topology.AddLocationResponse;
using AddZoneRequest = Nimbo.Wms.Models.Topology.AddZoneRequest;
using AddZoneResponse = Nimbo.Wms.Models.Topology.AddZoneResponse;

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
    public async Task<ActionResult<AddZoneResponse>> AddZone(
        [FromRoute] Guid warehouseGuid,
        [FromBody] AddZoneRequest request,
        CancellationToken ct)
    {
        var zoneGuid = await sender.Send(request with { WarehouseGuid = warehouseGuid }, ct);

        // Location header points to warehouse topology
        return CreatedAtAction(
            actionName: nameof(WarehousesController.GetWarehouseTopology),
            controllerName: "Warehouses",
            routeValues: new { warehouseGuid },
            value: new AddZoneResponse(zoneGuid));
    }

    /// <summary>
    /// Adds a new location to the specified warehouse based on the provided request parameters.
    /// </summary>
    /// <returns>
    /// An ActionResult containing the response with the newly created location's identifier.
    /// </returns>
    [HttpPost("locations")]
    [ProducesResponseType(typeof(AddLocationResponse), StatusCodes.Status201Created)]
    [Produces("application/json")]
    public async Task<ActionResult<AddLocationResponse>> AddLocation(
        [FromRoute] Guid warehouseGuid,
        [FromBody] AddLocationRequest request,
        CancellationToken ct)
    {
        var locationGuid = await sender.Send(request with { WarehouseGuid = warehouseGuid}, ct);

        return CreatedAtAction(
            actionName: nameof(WarehousesController.GetWarehouseTopology),
            controllerName: "Warehouses",
            routeValues: new { warehouseGuid },
            value: new AddLocationResponse(locationGuid));
    }
}
