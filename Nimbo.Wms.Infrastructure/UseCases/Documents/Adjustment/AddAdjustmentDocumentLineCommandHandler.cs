using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Documents.Adjustment.Commands;
using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Adjustment;

[PublicAPI]
public class AddAdjustmentDocumentLineCommandHandler : IRequestHandler<AddAdjustmentDocumentLineCommand, Guid>
{
    private readonly IAdjustmentDocumentRepository _repository;
    private readonly IItemRepository _itemRepository;
    private readonly IBatchRepository _batchRepository;

    public AddAdjustmentDocumentLineCommandHandler(
        IAdjustmentDocumentRepository repository,
        IItemRepository itemRepository,
        IBatchRepository batchRepository)
    {
        _repository = repository;
        _itemRepository = itemRepository;
        _batchRepository = batchRepository;
    }

    public async Task<Guid> Handle(AddAdjustmentDocumentLineCommand request, CancellationToken ct)
    {
        var documentId = AdjustmentDocumentId.From(request.DocumentId);
        var document = await _repository.GetByIdWithLinesAsync(documentId, ct);
        if (document is null)
            throw new NotFoundException($"Adjustment document with ID '{documentId}' not found");

        if (document.Version > request.DocumentVersion)
            throw new ConcurrencyException($"Document version mismatch. Expected: {document.Version}, Actual: {request.DocumentVersion}");

        var itemId = ItemId.From(request.ItemId);
        var item = await _itemRepository.GetByIdAsync(itemId, ct);
        if (item is null)
            throw new InvalidOperationException($"Item with ID '{itemId}' not found");

        if (request.BatchId is null && item.IsBatchManaged)
            throw new DomainException($"Batch is required for item {itemId}");

        var batchId = BatchId.From(request.BatchId!.Value);
        var batch = await _batchRepository.GetByIdAsync(batchId, ct);
        if (batch is null && item.IsBatchManaged)
            throw new DomainException($"Batch with ID '{batchId}' not found");

        var locationId = LocationId.From(request.LocationId);
        var uom = Enum.Parse<UnitOfMeasure>(request.Delta.Uom);
        var delta = new QuantityDelta(request.Delta.Value, uom);

        return document.AddLine(itemId, batchId, locationId, delta, request.Notes);
    }
}
