using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Documents.Relocation.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Relocation;

[PublicAPI]
public class DeleteRelocationDocumentCommandHandler : IRequestHandler<DeleteRelocationDocumentCommand>
{
    private readonly IRelocationDocumentRepository _repository;

    public DeleteRelocationDocumentCommandHandler(IRelocationDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteRelocationDocumentCommand request, CancellationToken ct)
    {
        var documentId = RelocationDocumentId.From(request.Id);
        var document = await _repository.GetByIdAsync(documentId, ct);

        if (document == null)
            throw new NotFoundException($"Relocation document with ID {documentId} not found");

        if (document.Version > request.Version)
            throw new ConcurrencyException($"Document version mismatch. Expected: {document.Version}, Actual: {request.Version}");

        document.EnsureCanBeEdited();
        await _repository.DeleteAsync(document, ct);
    }
}
