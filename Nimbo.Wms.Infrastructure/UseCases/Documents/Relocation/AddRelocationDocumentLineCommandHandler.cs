using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.Relocation.Commands;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Relocation;

[PublicAPI]
public class AddRelocationDocumentLineCommandHandler : IRequestHandler<AddRelocationDocumentLineCommand, Result<Guid>>
{
    private readonly IRelocationDocumentRepository _repository;
    private readonly IItemRepository _itemRepository;
    private readonly IStockLotRepository _stockLotRepository;

    public AddRelocationDocumentLineCommandHandler(
        IRelocationDocumentRepository repository,
        IItemRepository itemRepository,
        IStockLotRepository stockLotRepository)
    {
        _repository = repository;
        _itemRepository = itemRepository;
        _stockLotRepository = stockLotRepository;
    }

    public async Task<Result<Guid>> Handle(AddRelocationDocumentLineCommand request, CancellationToken ct)
    {
        var documentId = RelocationDocumentId.From(request.DocumentId);
        var document = await _repository.GetByIdWithLinesAsync(documentId, ct);
        if (document is null)
            return Error.NotFound("document.notfound", $"Relocation document with ID '{documentId}' not found");

        if (document.Version > request.DocumentVersion)
            return Error.Conflict("document.conflict", $"Document version mismatch. Expected: {document.Version}, Actual: {request.DocumentVersion}");

        var itemId = ItemId.From(request.ItemId);
        var item = await _itemRepository.GetByIdAsync(itemId, ct);
        if (item is null)
            return Error.NotFound("item.notfound", $"Item with ID '{itemId}' not found");

        StockLotId? stockLotId = null;
        if (request.StockLotId is not null)
        {
            var stockLot = await _stockLotRepository.GetByIdAsync(StockLotId.From(request.StockLotId.Value), ct);
            if (stockLot is null)
                return Error.NotFound("stock_lot.notfound", $"Stock lot with ID '{request.StockLotId}' not found");

            stockLotId = stockLot.Id;
        }

        var fromLocationId = LocationId.From(request.FromLocationId);
        var toLocationId = LocationId.From(request.ToLocationId);
        var uom = Enum.Parse<UnitOfMeasure>(request.Quantity.Uom);
        var quantity = new Quantity(request.Quantity.Value, uom);

        return document.AddLine(itemId, quantity, fromLocationId, toLocationId, stockLotId, request.Notes);
    }
}
