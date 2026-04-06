using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Stock.Requests;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Stock.Handlers;

[PublicAPI]
internal sealed class CreateBatchRequestHandler : IRequestHandler<CreateBatchRequest, BatchId>
{
    private readonly IItemRepository _itemRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IBatchRepository _batchRepository;

    public CreateBatchRequestHandler(
        IItemRepository itemRepository,
        ISupplierRepository supplierRepository,
        IBatchRepository batchRepository)
    {
        _itemRepository = itemRepository;
        _supplierRepository = supplierRepository;
        _batchRepository = batchRepository;
    }

    public async Task<BatchId> Handle(CreateBatchRequest request, CancellationToken ct = default)
    {
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
            request.ManufacturedAt?.UtcDateTime,
            request.ExpiryDate?.UtcDateTime,
            request.ReceivedAt?.UtcDateTime,
            request.Notes);

        await _batchRepository.AddAsync(batch, ct);

        return batchId;
    }
}
