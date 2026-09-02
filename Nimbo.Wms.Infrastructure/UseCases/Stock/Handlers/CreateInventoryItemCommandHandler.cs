using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Stock.Commands;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Infrastructure.UseCases.Stock.Handlers;

[PublicAPI]
internal sealed class CreateInventoryItemCommandHandler : IRequestHandler<CreateInventoryItemCommand, Guid>
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

    public async Task<Guid> Handle(CreateInventoryItemCommand command, CancellationToken ct = default)
    {
        var warehouseId = WarehouseId.From(command.WarehouseId);
        var warehouse = await _warehouseRepository.GetByIdAsync(warehouseId, ct);
        if (warehouse == null)
            throw new NotFoundException("Warehouse not found");

        var locationId = LocationId.From(command.LocationId);
        if (warehouse.Locations.All(l => l.Id != locationId))
            throw new NotFoundException("Location not found");

        var itemId = ItemId.From(command.ItemId);
        var item = await _itemRepository.GetByIdAsync(itemId, ct);
        if (item == null)
            throw new NotFoundException("Item not found");

        var stockLotId = StockLotId.From(command.StockLotId);
        var stockLot = await _stockLotRepository.GetByIdAsync(stockLotId, ct);
        if (stockLot == null)
            throw new NotFoundException("StockLot not found");

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

        return inventoryItemId;
    }
}
