using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.Receiving.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Receiving;

[PublicAPI]
public class DeleteReceivingDocumentLineCommandHandler : IRequestHandler<DeleteReceivingDocumentLineCommand, Result>
{
    private readonly IReceivingDocumentRepository _repository;

    public DeleteReceivingDocumentLineCommandHandler(IReceivingDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteReceivingDocumentLineCommand request, CancellationToken ct)
    {
        var documentId = ReceivingDocumentId.From(request.DocumentId);
        var document = await _repository.GetByIdWithLinesAsync(documentId, ct);
        if (document is null)
            return Error.NotFound("document.notfound", $"Receiving document with ID '{documentId}' not found");

        if (document.Version > request.DocumentVersion)
            return Error.Conflict("document.conflict", $"Document version mismatch. Expected: {document.Version}, Actual: {request.DocumentVersion}");

        document.RemoveLine(request.Id);

        return Result.Success();
    }
}
