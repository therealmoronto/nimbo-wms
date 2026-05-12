using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Stock.Requests;
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
    private readonly IBatchRepository _batchRepository;
    private readonly IInventoryItemRepository _inventoryItemRepository;

    public CreateInventoryItemCommandHandler(
        IWarehouseRepository warehouseRepository,
        IItemRepository itemRepository,
        IBatchRepository batchRepository,
        IInventoryItemRepository inventoryItemRepository)
    {
        _warehouseRepository = warehouseRepository;
        _itemRepository = itemRepository;
        _batchRepository = batchRepository;
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

        BatchId? batchId = null;
        if (command.BatchId.HasValue)
        {
            var batch = await _batchRepository.GetByIdAsync(BatchId.From(command.BatchId.Value), ct);
            if (batch == null)
                throw new NotFoundException("Batch not found");

            batchId = batch.Id;
        }

        var inventoryItemId = InventoryItemId.New();

        var quantity = new Quantity(command.Quantity, Enum.Parse<UnitOfMeasure>(command.QuantityUom));
        var status = Enum.Parse<InventoryStatus>(command.Status);

        var inventoryItem = new InventoryItem(
            inventoryItemId,
            itemId,
            warehouseId,
            locationId,
            quantity,
            status,
            batchId,
            command.SerialNumber,
            command.UnitCost);

        await _inventoryItemRepository.AddAsync(inventoryItem, ct);

        return inventoryItemId;
    }
}
