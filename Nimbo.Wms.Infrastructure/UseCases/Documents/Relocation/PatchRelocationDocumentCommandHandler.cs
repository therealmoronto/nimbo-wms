using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Documents.Relocation.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Relocation;

[PublicAPI]
public class PatchRelocationDocumentCommandHandler : IRequestHandler<PatchRelocationDocumentCommand>
{
    private readonly IRelocationDocumentRepository _repository;

    public PatchRelocationDocumentCommandHandler(IRelocationDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(PatchRelocationDocumentCommand request, CancellationToken ct)
    {
        var documentId = RelocationDocumentId.From(request.Id);
        var document = await _repository.GetByIdAsync(documentId, ct);
        if (document is null)
            throw new NotFoundException($"Relocation document with ID '{documentId}' not found.");

        if (document.Version > request.Version)
            throw new ConcurrencyException($"Document version mismatch. Expected: {document.Version}, Actual: {request.Version}");

        if (!string.IsNullOrWhiteSpace(request.Code))
            document.ChangeCode(request.Code);

        if (!string.IsNullOrWhiteSpace(request.Title))
            document.ChangeCode(request.Title);

        if (!string.IsNullOrWhiteSpace(request.Notes))
            document.ChangeNotes(request.Notes);
    }
}
