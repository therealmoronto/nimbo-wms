using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Documents.Relocation.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Relocation;

[PublicAPI]
public class DeleteRelocationDocumentLineCommandHandler : IRequestHandler<DeleteRelocationDocumentLineCommand>
{
    private readonly IRelocationDocumentRepository _repository;

    public DeleteRelocationDocumentLineCommandHandler(IRelocationDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteRelocationDocumentLineCommand request, CancellationToken ct)
    {
        var documentId = RelocationDocumentId.From(request.DocumentId);
        var document = await _repository.GetByIdWithLinesAsync(documentId, ct);
        if (document is null)
            throw new NotFoundException($"Relocation document with ID '{documentId}' not found");

        if (document.Version > request.DocumentVersion)
            throw new ConcurrencyException($"Document version mismatch. Expected: {document.Version}, Actual: {request.DocumentVersion}");

        document.RemoveLine(request.Id);
    }
}
