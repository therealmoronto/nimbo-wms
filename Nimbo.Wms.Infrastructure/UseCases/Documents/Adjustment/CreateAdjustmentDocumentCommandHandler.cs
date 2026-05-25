using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Contracts.Documents.Adjustment.Commands;
using Nimbo.Wms.Domain.Entities.Documents.Adjustment;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Adjustment;

[PublicAPI]
public class CreateAdjustmentDocumentCommandHandler : IRequestHandler<CreateAdjustmentDocumentCommand, Guid>
{
    private readonly IAdjustmentDocumentRepository _repository;

    public CreateAdjustmentDocumentCommandHandler(IAdjustmentDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateAdjustmentDocumentCommand request, CancellationToken ct)
    {
        var warehouseId = WarehouseId.From(request.WarehouseId);
        var documentId = AdjustmentDocumentId.New();

        var document = new AdjustmentDocument(documentId, warehouseId, request.Code, request.Title, DateTime.UtcNow);
        document.ChangeReason(request.ReasonCode, request.ReasonText);

        await _repository.AddAsync(document, ct);

        return documentId.Value;
    }
}
