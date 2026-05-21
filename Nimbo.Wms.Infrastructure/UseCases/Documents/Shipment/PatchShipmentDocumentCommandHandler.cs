using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Documents.Shipment.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Shipment;

[PublicAPI]
public class PatchShipmentDocumentCommandHandler : IRequestHandler<PatchShipmentDocumentCommand>
{
    private readonly IShipmentDocumentRepository _repository;

    public PatchShipmentDocumentCommandHandler(IShipmentDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(PatchShipmentDocumentCommand request, CancellationToken ct)
    {
        var documentId = ShipmentDocumentId.From(request.Id);
        var document = await _repository.GetByIdAsync(documentId, ct);
        if (document is null)
            throw new InvalidOperationException($"Shipment document with ID {request.Id} not found");

        if (document.Version > request.Version)
            throw new ConcurrencyException($"Document version mismatch. Expected: {document.Version}, Actual: {request.Version}");

        if (!string.IsNullOrEmpty(request.Code))
            document.ChangeCode(request.Code);

        if (!string.IsNullOrEmpty(request.Title))
            document.ChangeTitle(request.Title);

        if (request.Notes is not null)
            document.ChangeNotes(request.Notes);

    }
}