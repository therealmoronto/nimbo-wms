using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.Shipment.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Shipment;

[PublicAPI]
public class DeleteShipmentDocumentCommandHandler : IRequestHandler<DeleteShipmentDocumentCommand, Result>
{
    private readonly IShipmentDocumentRepository _repository;

    public DeleteShipmentDocumentCommandHandler(IShipmentDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteShipmentDocumentCommand request, CancellationToken ct)
    {
        var documentId = ShipmentDocumentId.From(request.Id);
        var document = await _repository.GetByIdAsync(documentId, ct);
        if (document is null)
            return Error.NotFound("document.notfound", $"Shipment document with ID '{documentId}' not found");

        if (document.Version > request.Version)
            return Error.Conflict("document.conflict", $"Document version mismatch. Expected: {document.Version}, Actual: {request.Version}");

        await _repository.DeleteAsync(document, ct);

        return Result.Success();
    }
}
