using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Documents.Shipment.Commands;
using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Shipment;

[PublicAPI]
public class AddShipmentDocumentLineCommandHandler : IRequestHandler<AddShipmentDocumentLineCommand, Guid>
{
    private readonly IShipmentDocumentRepository _repository;
    private readonly IBatchRepository _batchRepository;
    private readonly IItemRepository _itemRepository;

    public AddShipmentDocumentLineCommandHandler(
        IShipmentDocumentRepository repository,
        IBatchRepository batchRepository,
        IItemRepository itemRepository)
    {
        _repository = repository;
        _batchRepository = batchRepository;
        _itemRepository = itemRepository;
    }

    public async Task<Guid> Handle(AddShipmentDocumentLineCommand request, CancellationToken ct)
    {
        var documentId = ShipmentDocumentId.From(request.DocumentId);
        var uom = Enum.Parse<UnitOfMeasure>(request.RequestedQuantity.Uom);
        var requestedQuantity = new Quantity(request.RequestedQuantity.Value, uom);

        var document = await _repository.GetByIdWithLinesAsync(documentId, ct);
        if (document is null)
            throw new InvalidOperationException($"Shipment document with ID {request.DocumentId} not found");

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

        var lineId = document.AddRequestedLine(itemId, batchId, requestedQuantity, request.Notes);

        return lineId;
    }
}
