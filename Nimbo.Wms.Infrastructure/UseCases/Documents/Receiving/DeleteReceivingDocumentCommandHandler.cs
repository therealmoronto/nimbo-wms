using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Documents.Receiving.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Receiving;

[PublicAPI]
public class DeleteReceivingDocumentCommandHandler : IRequestHandler<DeleteReceivingDocumentCommand>
{
    private readonly IReceivingDocumentRepository _repository;

    public DeleteReceivingDocumentCommandHandler(IReceivingDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteReceivingDocumentCommand request, CancellationToken ct)
    {
        var documentId = ReceivingDocumentId.From(request.Id);
        var document = await _repository.GetByIdAsync(documentId, ct);

        if (document == null)
            throw new NotFoundException($"Receiving document with ID {documentId} not found");

        if (document.Version > request.Version)
            throw new ConcurrencyException($"Document version mismatch. Expected: {document.Version}, Actual: {request.Version}");

        document.EnsureCanBeEdited();
        await _repository.DeleteAsync(document, ct);
    }
}
