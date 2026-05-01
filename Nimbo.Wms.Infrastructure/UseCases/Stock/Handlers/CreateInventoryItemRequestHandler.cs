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
internal sealed class CreateInventoryItemRequestHandler : IRequestHandler<CreateInventoryItemRequest, Guid>
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IBatchRepository _batchRepository;
    private readonly IInventoryItemRepository _inventoryItemRepository;

    public CreateInventoryItemRequestHandler(
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

    public async Task<Guid> Handle(CreateInventoryItemRequest request, CancellationToken ct = default)
    {
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

        var quantity = new Quantity(request.Quantity, Enum.Parse<UnitOfMeasure>(request.QuantityUom));
        var status = Enum.Parse<InventoryStatus>(request.Status);

        var inventoryItem = new InventoryItem(
            inventoryItemId,
            itemId,
            warehouseId,
            locationId,
            quantity,
            status,
            batchId,
            request.SerialNumber,
            request.UnitCost);

        await _inventoryItemRepository.AddAsync(inventoryItem, ct);

        return inventoryItemId;
    }
}
