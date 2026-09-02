using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Documents.Shipment.Commands;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Shipment;

[PublicAPI]
public class AddShipmentDocumentLineCommandHandler : IRequestHandler<AddShipmentDocumentLineCommand, Guid>
{
    private readonly IShipmentDocumentRepository _repository;
    private readonly IStockLotRepository _stockLotRepository;

    public AddShipmentDocumentLineCommandHandler(IShipmentDocumentRepository repository, IStockLotRepository stockLotRepository)
    {
        _repository = repository;
        _stockLotRepository = stockLotRepository;
    }

    public async Task<Guid> Handle(AddShipmentDocumentLineCommand request, CancellationToken ct)
    {
        var documentId = ShipmentDocumentId.From(request.DocumentId);
        var itemId = ItemId.From(request.ItemId);
        var uom = Enum.Parse<UnitOfMeasure>(request.RequestedQuantity.Uom);
        var requestedQuantity = new Quantity(request.RequestedQuantity.Value, uom);

        StockLotId? stockLotId = null;
        if (request.StockLotId is not null)
        {
            var stockLot = await _stockLotRepository.GetByIdAsync(StockLotId.From(request.StockLotId.Value), ct);
            if (stockLot is null)
                throw new NotFoundException($"Stock lot with ID '{request.StockLotId}' not found");

            stockLotId = stockLot.Id;
        }

        var document = await _repository.GetByIdWithLinesAsync(documentId, ct);
        if (document is null)
            throw new InvalidOperationException($"Shipment document with ID {request.DocumentId} not found");

        if (document.Version > request.DocumentVersion)
            throw new ConcurrencyException($"Document version mismatch. Expected: {document.Version}, Actual: {request.DocumentVersion}");

        var lineId = document.AddRequestedLine(itemId, requestedQuantity, stockLotId, request.Notes);

        return lineId;
    }
}
