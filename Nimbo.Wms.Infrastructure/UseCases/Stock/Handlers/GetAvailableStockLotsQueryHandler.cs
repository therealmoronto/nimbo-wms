using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Contracts.Stock.Queries;
using Nimbo.Wms.Contracts.ValueObject;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Stock.Handlers;

/// <summary>
/// Lists available stock lots for an item (optionally narrowed to a warehouse/location), ordered
/// FEFO/FIFO: VendorLot.ExpiryDate ascending when present, else StockLot.ReceivedAt ascending. Pure
/// read query — no allocation logic; the caller picks a lot and passes its id explicitly to a pick line.
/// </summary>
[PublicAPI]
internal sealed class GetAvailableStockLotsQueryHandler : IRequestHandler<GetAvailableStockLotsQuery, Result<IReadOnlyList<AvailableStockLotDto>>>
{
    private readonly NimboWmsDbContext _dbContext;

    public GetAvailableStockLotsQueryHandler(NimboWmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<IReadOnlyList<AvailableStockLotDto>>> Handle(GetAvailableStockLotsQuery query, CancellationToken ct = default)
    {
        var itemId = ItemId.From(query.ItemId);

        var inventory = _dbContext.Set<InventoryItem>().AsNoTracking()
            .Where(i => i.ItemId == itemId && i.Quantity.Value > 0m);

        if (query.WarehouseId is not null)
        {
            var warehouseId = WarehouseId.From(query.WarehouseId.Value);
            inventory = inventory.Where(i => i.WarehouseId == warehouseId);
        }

        if (query.LocationId is not null)
        {
            var locationId = LocationId.From(query.LocationId.Value);
            inventory = inventory.Where(i => i.LocationId == locationId);
        }

        var stockLots = _dbContext.Set<StockLot>().AsNoTracking();
        var vendorLots = _dbContext.Set<VendorLot>().AsNoTracking();

        var rows = from i in inventory
            join sl in stockLots on i.StockLotId equals sl.Id
            join vl in vendorLots on sl.VendorLotId equals vl.Id into tmpVendorLots
            from vl in tmpVendorLots.DefaultIfEmpty()
            select new AvailableStockLotDto(
                sl.Id.Value,
                i.ItemId.Value,
                i.WarehouseId.Value,
                i.LocationId.Value,
                sl.VendorLotId != null ? sl.VendorLotId.Value.Value : (Guid?)null,
                vl != null ? vl.BatchNumber : null,
                vl != null ? vl.ExpiryDate : null,
                sl.ReceivedAt,
                new QuantityDto { Value = i.Quantity.Value, Uom = i.Quantity.Uom.ToString(), IsZero = i.Quantity.IsZero });

        var results = await rows.ToListAsync(ct);

        return results
            .OrderBy(r => r.VendorLotExpiryDate ?? DateTime.MaxValue)
            .ThenBy(r => r.ReceivedAt)
            .ToList();
    }
}
