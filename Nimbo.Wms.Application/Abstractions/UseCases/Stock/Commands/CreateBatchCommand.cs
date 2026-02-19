using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Stock.Http;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Stock.Commands;

public sealed record CreateBatchCommand(CreateBatchRequest Request) : ICommand<BatchId>;

public sealed class CreateBatchHandler : ICommandHandler<CreateBatchCommand, BatchId>
{
    private readonly IItemRepository _itemRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IBatchRepository _batchRepository;
    private readonly IUnitOfWork _uow;

    public CreateBatchHandler(
        IItemRepository itemRepository,
        ISupplierRepository supplierRepository,
        IBatchRepository batchRepository,
        IUnitOfWork uow)
    {
        _itemRepository = itemRepository;
        _supplierRepository = supplierRepository;
        _batchRepository = batchRepository;
        _uow = uow;
    }

    public async Task<BatchId> HandleAsync(CreateBatchCommand command, CancellationToken ct = default)
    {
        var request = command.Request;
        var itemId = ItemId.From(request.ItemId);
        var item = await _itemRepository.GetByIdAsync(itemId, ct);
        if (item == null)
            throw new NotFoundException("Item not found");
        
        SupplierId? supplierId = null;
        if (request.SupplierId.HasValue)
        {
            var supplier = await _supplierRepository.GetByIdAsync(SupplierId.From(request.SupplierId.Value), ct);
            if (supplier == null)
                throw new NotFoundException("Supplier not found");

            supplierId = supplier.Id;
        }

        var batchId = BatchId.New();
        var batch = new Batch(batchId,
            itemId,
            request.BatchNumber,
            supplierId,
            request.ManufacturedAt,
            request.ExpiryDate,
            request.ReceivedAt,
            request.Notes);

        await _batchRepository.AddAsync(batch, ct);
        await _uow.SaveChangesAsync(ct);

        return batchId;
    }
}
