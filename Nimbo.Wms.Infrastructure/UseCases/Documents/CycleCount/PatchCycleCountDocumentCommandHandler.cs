using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Documents.CycleCount.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.CycleCount;

[PublicAPI]
public class PatchCycleCountDocumentCommandHandler : IRequestHandler<PatchCycleCountDocumentCommand>
{
    private readonly ICycleCountDocumentRepository _repository;

    public PatchCycleCountDocumentCommandHandler(ICycleCountDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(PatchCycleCountDocumentCommand request, CancellationToken ct)
    {
        var documentId = CycleCountDocumentId.From(request.Id);
        var document = await _repository.GetByIdAsync(documentId, ct);
        if (document is null)
            throw new NotFoundException($"Cycle count document with ID '{documentId}' not found.");

        if (document.Version > request.Version)
            throw new ConcurrencyException($"Document version mismatch. Expected: {document.Version}, Actual: {request.Version}");

        if (!string.IsNullOrWhiteSpace(request.Code))
            document.ChangeCode(request.Code);

        if (!string.IsNullOrWhiteSpace(request.Title))
            document.ChangeTitle(request.Title);
    }
}
