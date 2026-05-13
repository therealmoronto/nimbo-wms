using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Stock.Commands;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Stock.Handlers;

[PublicAPI]
internal sealed class CreateBatchCommandHandler : IRequestHandler<CreateBatchCommand, Guid>
{
    private readonly IItemRepository _itemRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IBatchRepository _batchRepository;

    public CreateBatchCommandHandler(
        IItemRepository itemRepository,
        ISupplierRepository supplierRepository,
        IBatchRepository batchRepository)
    {
        _itemRepository = itemRepository;
        _supplierRepository = supplierRepository;
        _batchRepository = batchRepository;
    }

    public async Task<Guid> Handle(CreateBatchCommand command, CancellationToken ct = default)
    {
        var itemId = ItemId.From(command.ItemId);
        var item = await _itemRepository.GetByIdAsync(itemId, ct);
        if (item == null)
            throw new NotFoundException("Item not found");

        SupplierId? supplierId = null;
        if (command.SupplierId.HasValue)
        {
            var supplier = await _supplierRepository.GetByIdAsync(SupplierId.From(command.SupplierId.Value), ct);
            if (supplier == null)
                throw new NotFoundException("Supplier not found");

            supplierId = supplier.Id;
        }

        var batchId = BatchId.New();
        var batch = new Batch(batchId,
            itemId,
            command.BatchNumber,
            supplierId,
            command.ManufacturedAt?.UtcDateTime,
            command.ExpiryDate?.UtcDateTime,
            command.ReceivedAt?.UtcDateTime,
            command.Notes);

        await _batchRepository.AddAsync(batch, ct);

        return batchId;
    }
}
