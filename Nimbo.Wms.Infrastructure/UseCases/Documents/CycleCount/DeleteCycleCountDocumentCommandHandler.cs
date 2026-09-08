using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.CycleCount.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.CycleCount;

[PublicAPI]
public class DeleteCycleCountDocumentCommandHandler : IRequestHandler<DeleteCycleCountDocumentCommand, Result>
{
    private readonly ICycleCountDocumentRepository _repository;

    public DeleteCycleCountDocumentCommandHandler(ICycleCountDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteCycleCountDocumentCommand request, CancellationToken ct)
    {
        var documentId = CycleCountDocumentId.From(request.Id);
        var document = await _repository.GetByIdAsync(documentId, ct);

        if (document == null)
            return Error.NotFound("document.notfound", $"Cycle count document with ID {documentId} not found");

        if (document.Version > request.Version)
            return Error.Conflict("document.conflict", $"Document version mismatch. Expected: {document.Version}, Actual: {request.Version}");

        document.EnsureCanBeEdited();
        await _repository.DeleteAsync(document, ct);
        return Result.Success();
    }
}
