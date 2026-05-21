using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Contracts.Documents.CycleCount.Commands;
using Nimbo.Wms.Domain.Entities.Documents.CycleCount;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.CycleCount;

[PublicAPI]
public class CreateCycleCountDocumentCommandHandler : IRequestHandler<CreateCycleCountDocumentCommand, Guid>
{
    private readonly ICycleCountDocumentRepository _repository;

    public CreateCycleCountDocumentCommandHandler(ICycleCountDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateCycleCountDocumentCommand request, CancellationToken ct)
    {
        var warehouseId = WarehouseId.From(request.WarehouseId);
        var documentId = CycleCountDocumentId.New();

        var document = new CycleCountDocument(documentId, warehouseId, request.Code, request.Title, DateTime.UtcNow);
        await _repository.AddAsync(document, ct);

        return documentId;
    }
}
