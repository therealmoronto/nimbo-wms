using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Documents.Shipment.Commands;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Shipment;

[PublicAPI]
public class AddPickLineCommandHandler : IRequestHandler<AddPickLineCommand, Guid>
{
    private readonly IShipmentDocumentRepository _repository;

    public AddPickLineCommandHandler(IShipmentDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(AddPickLineCommand request, CancellationToken ct)
    {
        var documentId = ShipmentDocumentId.From(request.DocumentId);
        var itemId = ItemId.From(request.ItemId);
        var fromLocationId = LocationId.From(request.FromLocationId);
        var uom = Enum.Parse<UnitOfMeasure>(request.Quantity.Uom);
        var quantity = new Quantity(request.Quantity.Value, uom);

        var document = await _repository.GetByIdWithLinesAsync(documentId, ct);
        if (document is null)
            throw new InvalidOperationException($"Shipment document with ID {request.DocumentId} not found");

        if (document.Version > request.DocumentVersion)
            throw new ConcurrencyException($"Document version mismatch. Expected: {document.Version}, Actual: {request.DocumentVersion}");

        return document.AddPickLine(itemId, fromLocationId, quantity, request.Notes);
    }
}