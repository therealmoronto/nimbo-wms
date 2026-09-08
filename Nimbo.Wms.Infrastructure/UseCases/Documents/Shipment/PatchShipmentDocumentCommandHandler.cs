using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.Shipment.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Shipment;

[PublicAPI]
public class PatchShipmentDocumentCommandHandler : IRequestHandler<PatchShipmentDocumentCommand, Result>
{
    private readonly IShipmentDocumentRepository _repository;

    public PatchShipmentDocumentCommandHandler(IShipmentDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(PatchShipmentDocumentCommand request, CancellationToken ct)
    {
        var documentId = ShipmentDocumentId.From(request.Id);
        var document = await _repository.GetByIdAsync(documentId, ct);
        if (document is null)
            return Error.NotFound("document.notfound", $"Shipment document with ID '{documentId}' not found");

        if (document.Version > request.Version)
            return Error.Conflict("document.conflict", $"Document version mismatch. Expected: {document.Version}, Actual: {request.Version}");

        if (!string.IsNullOrEmpty(request.Code))
            document.ChangeCode(request.Code);

        if (!string.IsNullOrEmpty(request.Title))
            document.ChangeTitle(request.Title);

        if (request.Notes is not null)
            document.ChangeNotes(request.Notes);

        return Result.Success();
    }
}
