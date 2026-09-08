using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.Relocation.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Relocation;

[PublicAPI]
public class DeleteRelocationDocumentCommandHandler : IRequestHandler<DeleteRelocationDocumentCommand, Result>
{
    private readonly IRelocationDocumentRepository _repository;

    public DeleteRelocationDocumentCommandHandler(IRelocationDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteRelocationDocumentCommand request, CancellationToken ct)
    {
        var documentId = RelocationDocumentId.From(request.Id);
        var document = await _repository.GetByIdAsync(documentId, ct);

        if (document is null)
            return Error.NotFound("document.notfound", $"Relocation document with ID '{documentId}' not found");

        if (document.Version > request.Version)
            return Error.Conflict("document.conflict", $"Document version mismatch. Expected: {document.Version}, Actual: {request.Version}");

        document.EnsureCanBeEdited();
        await _repository.DeleteAsync(document, ct);

        return Result.Success();
    }
}
