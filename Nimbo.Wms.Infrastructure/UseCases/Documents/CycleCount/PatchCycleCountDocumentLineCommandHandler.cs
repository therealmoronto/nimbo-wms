using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Documents.CycleCount.Commands;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.CycleCount;

[PublicAPI]
public class PatchCycleCountDocumentLineCommandHandler : IRequestHandler<PatchCycleCountDocumentLineCommand>
{
    private readonly ICycleCountDocumentRepository _repository;

    public PatchCycleCountDocumentLineCommandHandler(ICycleCountDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(PatchCycleCountDocumentLineCommand request, CancellationToken ct)
    {
        var documentId = CycleCountDocumentId.From(request.DocumentId);
        var document = await _repository.GetByIdWithLinesAsync(documentId, ct);
        if (document is null)
            throw new NotFoundException($"Cycle count document with ID '{documentId}' not found");

        if (document.Version > request.DocumentVersion)
            throw new ConcurrencyException($"Document version mismatch. Expected: {document.Version}, Actual: {request.DocumentVersion}");

        var lineId = request.LineId;
        if (request.ExpectedQuantity is not null)
        {
            var uom = Enum.Parse<UnitOfMeasure>(request.ExpectedQuantity.Uom);
            var actualQuantity = new Quantity(request.ExpectedQuantity.Value, uom);
            document.ChangeLineActualQuantity(lineId, actualQuantity);
        }

        if (request.Notes is not null)
            document.ChangeLineNotes(lineId, request.Notes);
    }
}
