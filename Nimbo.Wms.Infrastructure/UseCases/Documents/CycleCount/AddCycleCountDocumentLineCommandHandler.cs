using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Documents.CycleCount.Commands;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.CycleCount;

[PublicAPI]
public class AddCycleCountDocumentLineCommandHandler : IRequestHandler<AddCycleCountDocumentLineCommand, Guid>
{
    private readonly ICycleCountDocumentRepository _repository;
    private readonly IItemRepository _itemRepository;
    private readonly IStockLotRepository _stockLotRepository;

    public AddCycleCountDocumentLineCommandHandler(
        ICycleCountDocumentRepository repository,
        IItemRepository itemRepository,
        IStockLotRepository stockLotRepository)
    {
        _repository = repository;
        _itemRepository = itemRepository;
        _stockLotRepository = stockLotRepository;
    }

    public async Task<Guid> Handle(AddCycleCountDocumentLineCommand request, CancellationToken ct)
    {
        var documentId = CycleCountDocumentId.From(request.DocumentId);
        var document = await _repository.GetByIdWithLinesAsync(documentId, ct);
        if (document is null)
            throw new NotFoundException($"Cycle count document with ID '{documentId}' not found");

        if (document.Version > request.DocumentVersion)
            throw new ConcurrencyException($"Document version mismatch. Expected: {document.Version}, Actual: {request.DocumentVersion}");

        var itemId = ItemId.From(request.ItemId);
        var item = await _itemRepository.GetByIdAsync(itemId, ct);
        if (item is null)
            throw new InvalidOperationException($"Item with ID '{itemId}' not found");

        StockLotId? stockLotId = null;
        if (request.StockLotId is not null)
        {
            var stockLot = await _stockLotRepository.GetByIdAsync(StockLotId.From(request.StockLotId.Value), ct);
            if (stockLot is null)
                throw new NotFoundException($"Stock lot with ID '{request.StockLotId}' not found");

            stockLotId = stockLot.Id;
        }

        var locationId = LocationId.From(request.LocationId);
        var uom = Enum.Parse<UnitOfMeasure>(request.ExpectedQuantity.Uom);
        var expectedQuantity = new Quantity(request.ExpectedQuantity.Value, uom);

        return document.AddLine(itemId, locationId, expectedQuantity, stockLotId);
    }
}
