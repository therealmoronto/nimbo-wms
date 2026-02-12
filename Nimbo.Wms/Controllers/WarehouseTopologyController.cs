using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;
using Nimbo.Wms.Contracts.Topology.Http;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Controllers;

[ApiController]
[Route("api/topology/warehouses/{warehouseGuid:guid}")]
public class WarehouseTopologyController : ControllerBase
{
    [HttpPost("zones")]
    [ProducesResponseType(typeof(AddZoneResponse), StatusCodes.Status201Created)]
    [Produces("application/json")]
    public async Task<ActionResult<AddZoneResponse>> AddZone(
        [FromRoute] Guid warehouseGuid,
        [FromBody] AddZoneRequest request,
        [FromServices] ICommandHandler<AddZoneToWarehouseCommand, ZoneId> handler,
        CancellationToken ct)
    {
        var warehouseId = WarehouseId.From(warehouseGuid);
        var command = new AddZoneToWarehouseCommand(warehouseId, request.Code, request.Name, request.Type);
        var zoneId = await handler.HandleAsync(command, ct);

        // Location header points to warehouse topology
        return CreatedAtAction(
            actionName: nameof(WarehousesController.GetWarehouseTopology),
            controllerName: "Warehouses",
            routeValues: new { warehouseGuid },
            value: new AddZoneResponse(zoneId.Value));
    }

    [HttpPost("locations")]
    [ProducesResponseType(typeof(AddLocationResponse), StatusCodes.Status201Created)]
    [Produces("application/json")]
    public async Task<ActionResult<AddLocationResponse>> AddLocation(
        [FromRoute] Guid warehouseGuid,
        [FromBody] AddLocationRequest request,
        [FromServices] ICommandHandler<AddLocationToWarehouseCommand, LocationId> handler,
        CancellationToken ct)
    {
        var warehouseId = WarehouseId.From(warehouseGuid);
        var zoneId = ZoneId.From(request.ZoneId);
        var command = new AddLocationToWarehouseCommand(warehouseId, zoneId, request.Code, request.Type);
        var locationId = await handler.HandleAsync(command, ct);

        return CreatedAtAction(
            actionName: nameof(WarehousesController.GetWarehouseTopology),
            controllerName: "Warehouses",
            routeValues: new { warehouseGuid },
            value: new AddLocationResponse(locationId.Value));
    }
}
