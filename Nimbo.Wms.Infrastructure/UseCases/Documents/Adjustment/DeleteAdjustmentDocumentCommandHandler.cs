using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Documents.Adjustment.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Adjustment;

[PublicAPI]
public class DeleteAdjustmentDocumentCommandHandler : IRequestHandler<DeleteAdjustmentDocumentCommand>
{
    private readonly IAdjustmentDocumentRepository _repository;

    public DeleteAdjustmentDocumentCommandHandler(IAdjustmentDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteAdjustmentDocumentCommand request, CancellationToken ct)
    {
        var documentId = AdjustmentDocumentId.From(request.Id);
        var document = await _repository.GetByIdWithLinesAsync(documentId, ct);
        if (document is null)
            throw new NotFoundException($"Adjustment document with ID '{documentId}' not found");

        if (document.Version > request.Version)
            throw new ConcurrencyException($"Document version mismatch. Expected: {document.Version}, Actual: {request.Version}");

        document.EnsureCanBeEdited();
        await _repository.DeleteAsync(document, ct);
    }
}
