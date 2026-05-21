using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Documents.Shipment.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Shipment;

[PublicAPI]
public class DeleteShipmentDocumentCommandHandler : IRequestHandler<DeleteShipmentDocumentCommand>
{
    private readonly IShipmentDocumentRepository _repository;

    public DeleteShipmentDocumentCommandHandler(IShipmentDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteShipmentDocumentCommand request, CancellationToken ct)
    {
        var documentId = ShipmentDocumentId.From(request.Id);
        var document = await _repository.GetByIdAsync(documentId, ct);
        if (document is null)
            throw new InvalidOperationException($"Shipment document with ID {request.Id} not found");

        if (document.Version > request.Version)
            throw new ConcurrencyException($"Document version mismatch. Expected: {document.Version}, Actual: {request.Version}");

        await _repository.DeleteAsync(document, ct);
    }
}