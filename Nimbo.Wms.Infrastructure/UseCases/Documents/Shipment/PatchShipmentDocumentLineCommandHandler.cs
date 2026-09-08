using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.Shipment.Commands;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Shipment;

[PublicAPI]
public class PatchShipmentDocumentLineCommandHandler : IRequestHandler<PatchShipmentDocumentLineCommand, Result>
{
    private readonly IShipmentDocumentRepository _repository;

    public PatchShipmentDocumentLineCommandHandler(IShipmentDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(PatchShipmentDocumentLineCommand request, CancellationToken ct)
    {
        var documentId = ShipmentDocumentId.From(request.DocumentId);
        var document = await _repository.GetByIdWithLinesAsync(documentId, ct);
        if (document is null)
            return Error.NotFound("document.notfound", $"Shipment document with ID '{documentId}' not found");

        if (document.Version > request.DocumentVersion)
            return Error.Conflict("document.conflict", $"Document version mismatch. Expected: {document.Version}, Actual: {request.DocumentVersion}");

        if (request.RequestedQuantity is not null)
        {
            var uom = Enum.Parse<UnitOfMeasure>(request.RequestedQuantity.Uom);
            var requestedQuantity = new Quantity(request.RequestedQuantity.Value, uom);
            document.ChangeLineRequestedQuantity(request.Id, requestedQuantity);
        }

        if (request.Notes is not null)
        {
            document.ChangeLineNotes(request.Id, request.Notes);
        }

        return Result.Success();
    }
}
