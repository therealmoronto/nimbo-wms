using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Contracts.Documents.Shipment.Commands;
using Nimbo.Wms.Domain.Entities.Documents.Shipment;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Shipment;

[PublicAPI]
public class CreateShipmentDocumentCommandHandler : IRequestHandler<CreateShipmentDocumentCommand, Guid>
{
    private readonly IShipmentDocumentRepository _repository;

    public CreateShipmentDocumentCommandHandler(IShipmentDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateShipmentDocumentCommand request, CancellationToken ct)
    {
        var warehouseId = WarehouseId.From(request.WarehouseId);
        var documentId = ShipmentDocumentId.New();

        var document = new ShipmentDocument(documentId, warehouseId, request.Code, request.Title, DateTime.UtcNow);
        await _repository.AddAsync(document, ct);

        return documentId;
    }
}
