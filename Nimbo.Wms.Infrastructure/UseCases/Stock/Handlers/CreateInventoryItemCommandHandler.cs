using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Stock.Commands;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Infrastructure.UseCases.Stock.Handlers;

[PublicAPI]
internal sealed class CreateInventoryItemCommandHandler : IRequestHandler<CreateInventoryItemCommand, Result<Guid>>
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IStockLotRepository _stockLotRepository;
    private readonly IInventoryItemRepository _inventoryItemRepository;

    public CreateInventoryItemCommandHandler(
        IWarehouseRepository warehouseRepository,
        IItemRepository itemRepository,
        IStockLotRepository stockLotRepository,
        IInventoryItemRepository inventoryItemRepository)
    {
        _warehouseRepository = warehouseRepository;
        _itemRepository = itemRepository;
        _stockLotRepository = stockLotRepository;
        _inventoryItemRepository = inventoryItemRepository;
    }

    public async Task<Result<Guid>> Handle(CreateInventoryItemCommand command, CancellationToken ct = default)
    {
        var warehouseId = WarehouseId.From(command.WarehouseId);
        var warehouse = await _warehouseRepository.GetByIdAsync(warehouseId, ct);
        if (warehouse == null)
            return Error.NotFound("warehouse.notfound", "Warehouse not found");

        var locationId = LocationId.From(command.LocationId);
        if (warehouse.Locations.All(l => l.Id != locationId))
            return Error.NotFound("location.notfound", "Location not found");

        var itemId = ItemId.From(command.ItemId);
        var item = await _itemRepository.GetByIdAsync(itemId, ct);
        if (item == null)
            return Error.NotFound("item.notfound", "Item not found");

        var stockLotId = StockLotId.From(command.StockLotId);
        var stockLot = await _stockLotRepository.GetByIdAsync(stockLotId, ct);
        if (stockLot == null)
            return Error.NotFound("stocklot.notfound", "StockLot not found");

        var inventoryItemId = InventoryItemId.New();

        var quantity = new Quantity(command.Quantity, Enum.Parse<UnitOfMeasure>(command.QuantityUom));
        var status = Enum.Parse<InventoryStatus>(command.Status);

        var inventoryItem = new InventoryItem(
            inventoryItemId,
            itemId,
            warehouseId,
            locationId,
            stockLotId,
            quantity,
            status,
            command.SerialNumber,
            command.UnitCost);

        await _inventoryItemRepository.AddAsync(inventoryItem, ct);

        return inventoryItemId.Value;
    }
}
