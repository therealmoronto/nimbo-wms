using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Stock.Http;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Stock.Commands;

public sealed record CreateInventoryItemCommand(CreateInventoryItemRequest Request) : ICommand<InventoryItemId>;

public sealed class CreateInventoryItemHandler : ICommandHandler<CreateInventoryItemCommand, InventoryItemId>
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IBatchRepository _batchRepository;
    private readonly IInventoryItemRepository _inventoryItemRepository;
    private readonly IUnitOfWork _uow;

    public CreateInventoryItemHandler(
        IWarehouseRepository warehouseRepository,
        IItemRepository itemRepository,
        IBatchRepository batchRepository,
        IInventoryItemRepository inventoryItemRepository,
        IUnitOfWork uow)
    {
        _warehouseRepository = warehouseRepository;
        _itemRepository = itemRepository;
        _batchRepository = batchRepository;
        _inventoryItemRepository = inventoryItemRepository;
        _uow = uow;
    }

    public async Task<InventoryItemId> HandleAsync(CreateInventoryItemCommand command, CancellationToken ct = default)
    {
        var request = command.Request;
        var warehouseId = WarehouseId.From(request.WarehouseId);
        var warehouse = await _warehouseRepository.GetByIdAsync(warehouseId, ct);
        if (warehouse == null)
            throw new NotFoundException("Warehouse not found");
        
        var locationId = LocationId.From(request.LocationId);
        if (warehouse.Locations.All(l => l.Id != locationId))
            throw new NotFoundException("Location not found");
        
        var itemId = ItemId.From(request.ItemId);
        var item = await _itemRepository.GetByIdAsync(itemId, ct);
        if (item == null)
            throw new NotFoundException("Item not found");
        
        BatchId? batchId = null;
        if (request.BatchId.HasValue)
        {
            var batch = await _batchRepository.GetByIdAsync(BatchId.From(request.BatchId.Value), ct);
            if (batch == null)
                throw new NotFoundException("Batch not found");
            
            batchId = batch.Id;
        }

        var inventoryItemId = InventoryItemId.New();

        var inventoryItem = new InventoryItem(
            inventoryItemId,
            itemId,
            warehouseId,
            locationId,
            request.Quantity,
            request.Status,
            batchId,
            request.SerialNumber,
            request.UnitCost);

        await _inventoryItemRepository.AddAsync(inventoryItem, ct);
        await _uow.SaveChangesAsync(ct);
        
        return inventoryItemId;
    }
}
