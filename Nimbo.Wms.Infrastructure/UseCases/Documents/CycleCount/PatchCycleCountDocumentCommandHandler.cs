using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.CycleCount.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.CycleCount;

[PublicAPI]
public class PatchCycleCountDocumentCommandHandler : IRequestHandler<PatchCycleCountDocumentCommand, Result>
{
    private readonly ICycleCountDocumentRepository _repository;

    public PatchCycleCountDocumentCommandHandler(ICycleCountDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(PatchCycleCountDocumentCommand request, CancellationToken ct)
    {
        var documentId = CycleCountDocumentId.From(request.Id);
        var document = await _repository.GetByIdAsync(documentId, ct);
        if (document is null)
            return Error.NotFound("document.notfound", $"Cycle count document with ID '{documentId}' not found.");

        if (document.Version > request.Version)
            return Error.Conflict("document.conflict", $"Document version mismatch. Expected: {document.Version}, Actual: {request.Version}");

        if (!string.IsNullOrWhiteSpace(request.Code))
            document.ChangeCode(request.Code);

        if (!string.IsNullOrWhiteSpace(request.Title))
            document.ChangeTitle(request.Title);

        return Result.Success();
    }
}
