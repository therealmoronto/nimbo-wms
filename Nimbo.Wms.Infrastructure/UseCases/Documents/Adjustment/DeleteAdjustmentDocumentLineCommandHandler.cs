using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.Adjustment.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Adjustment;

[PublicAPI]
public class DeleteAdjustmentDocumentLineCommandHandler : IRequestHandler<DeleteAdjustmentDocumentLineCommand, Result>
{
    private readonly IAdjustmentDocumentRepository _repository;

    public DeleteAdjustmentDocumentLineCommandHandler(IAdjustmentDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteAdjustmentDocumentLineCommand request, CancellationToken ct)
    {
        var documentId = AdjustmentDocumentId.From(request.DocumentId);
        var document = await _repository.GetByIdWithLinesAsync(documentId, ct);
        if (document is null)
            return Error.NotFound("document.notfound", $"Adjustment document with ID '{documentId}' not found");

        if (document.Version > request.DocumentVersion)
            return Error.Conflict("document.conflict", $"Document version mismatch. Expected: {document.Version}, Actual: {request.DocumentVersion}");

        document.RemoveLine(request.Id);

        return Result.Success();
    }
}
