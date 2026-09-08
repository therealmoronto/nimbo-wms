using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.CycleCount.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.CycleCount;

[PublicAPI]
public class DeleteCycleCountDocumentLineCommandHandler : IRequestHandler<DeleteCycleCountDocumentLineCommand, Result>
{
    private readonly ICycleCountDocumentRepository _repository;

    public DeleteCycleCountDocumentLineCommandHandler(ICycleCountDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteCycleCountDocumentLineCommand request, CancellationToken ct)
    {
        var documentId = CycleCountDocumentId.From(request.DocumentId);
        var document = await _repository.GetByIdWithLinesAsync(documentId, ct);

        if (document == null)
            return Error.NotFound("document.notfound", $"Cycle count document with ID '{documentId}' not found");

        if (document.Version > request.DocumentVersion)
            return Error.Conflict("document.conflict", $"Document version mismatch. Expected: {document.Version}, Actual: {request.DocumentVersion}");

        var lineId = request.Id;
        document.RemoveLine(lineId);
        return Result.Success();
    }
}
