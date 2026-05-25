using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Documents.Receiving.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Receiving;

[PublicAPI]
public class DeleteReceivingDocumentLineCommandHandler : IRequestHandler<DeleteReceivingDocumentLineCommand>
{
    private readonly IReceivingDocumentRepository _repository;

    public DeleteReceivingDocumentLineCommandHandler(IReceivingDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteReceivingDocumentLineCommand request, CancellationToken ct)
    {
        var documentId = ReceivingDocumentId.From(request.DocumentId);
        var document = await _repository.GetByIdWithLinesAsync(documentId, ct);
        if (document is null)
            throw new NotFoundException($"Receiving document with ID '{documentId}' not found");

        if (document.Version > request.DocumentVersion)
            throw new ConcurrencyException($"Document version mismatch. Expected: {document.Version}, Actual: {request.DocumentVersion}");

        document.RemoveLine(request.Id);
    }
}
