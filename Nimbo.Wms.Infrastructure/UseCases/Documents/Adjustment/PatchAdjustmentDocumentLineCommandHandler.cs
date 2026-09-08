using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.Adjustment.Commands;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Adjustment;

[PublicAPI]
public class PatchAdjustmentDocumentLineCommandHandler : IRequestHandler<PatchAdjustmentDocumentLineCommand, Result>
{
    private readonly IAdjustmentDocumentRepository _repository;

    public PatchAdjustmentDocumentLineCommandHandler(IAdjustmentDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(PatchAdjustmentDocumentLineCommand request, CancellationToken ct)
    {
        var documentId = AdjustmentDocumentId.From(request.DocumentId);
        var document = await _repository.GetByIdWithLinesAsync(documentId, ct);
        if (document is null)
            return Error.NotFound("document.notfound", $"Adjustment document with ID '{documentId}' not found");

        if (document.Version > request.DocumentVersion)
            return Error.Conflict("document.conflict", $"Document version mismatch. Expected: {document.Version}, Actual: {request.DocumentVersion}");

        if (request.Delta is not null)
        {
            var uom = Enum.Parse<UnitOfMeasure>(request.Delta.Uom);
            var delta = new QuantityDelta(request.Delta.Value, uom);
            document.ChangeLineDelta(request.Id, delta);
        }

        if (request.LocationId.HasValue)
        {
            document.ChangeLineLocation(request.Id, LocationId.From(request.LocationId.Value));
        }

        if (request.Notes is not null)
        {
            document.ChangeLineNotes(request.Id, request.Notes);
        }

        return Result.Success();
    }
}
