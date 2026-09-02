using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Contracts.Stock.Queries;

namespace Nimbo.Wms.Controllers.Stock;

[ApiController]
[Route("api/stock")]
public class StockLotsController : ControllerBase
{
    /// <summary>
    /// Lists available stock lots for an item (optionally narrowed to a warehouse/location), ordered
    /// FEFO/FIFO: VendorLot expiry ascending when present, else StockLot receipt date ascending.
    /// </summary>
    /// <remarks>
    /// Pure read query — there is no auto-allocation engine. Use this to choose a StockLotId when
    /// creating a shipment pick line (or any other line that references a stock lot).
    /// </remarks>
    /// <returns>
    /// A list of <see cref="AvailableStockLotDto"/> ordered by FEFO/FIFO. Empty if no stock is available.
    /// </returns>
    [HttpGet("available-stock-lots")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<IReadOnlyList<AvailableStockLotDto>> GetAvailableStockLots(
        [FromQuery] Guid itemGuid,
        [FromQuery] Guid? warehouseGuid,
        [FromQuery] Guid? locationGuid,
        [FromServices] IMediator mediator,
        CancellationToken ct)
    {
        var query = new GetAvailableStockLotsQuery(itemGuid, warehouseGuid, locationGuid);
        return await mediator.Send(query, ct);
    }
}
