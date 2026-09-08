using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.Receiving.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Receiving;

[PublicAPI]
public class DeleteReceivingDocumentCommandHandler : IRequestHandler<DeleteReceivingDocumentCommand, Result>
{
    private readonly IReceivingDocumentRepository _repository;

    public DeleteReceivingDocumentCommandHandler(IReceivingDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteReceivingDocumentCommand request, CancellationToken ct)
    {
        var documentId = ReceivingDocumentId.From(request.Id);
        var document = await _repository.GetByIdAsync(documentId, ct);

        if (document is null)
            return Error.NotFound("document.notfound", $"Receiving document with ID '{documentId}' not found");

        if (document.Version > request.Version)
            return Error.Conflict("document.conflict", $"Document version mismatch. Expected: {document.Version}, Actual: {request.Version}");

        document.EnsureCanBeEdited();
        await _repository.DeleteAsync(document, ct);

        return Result.Success();
    }
}
