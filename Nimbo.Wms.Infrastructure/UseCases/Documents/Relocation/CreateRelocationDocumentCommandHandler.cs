using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Contracts.Documents.Relocation.Commands;
using Nimbo.Wms.Domain.Entities.Documents.Relocation;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Relocation;

[PublicAPI]
public class CreateRelocationDocumentCommandHandler : IRequestHandler<CreateRelocationDocumentCommand, Guid>
{
    private readonly IRelocationDocumentRepository _repository;

    public CreateRelocationDocumentCommandHandler(IRelocationDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateRelocationDocumentCommand request, CancellationToken ct)
    {
        var warehouseId = WarehouseId.From(request.WarehouseId);
        var documentId = RelocationDocumentId.New();

        var document = new RelocationDocument(documentId, warehouseId, request.Code, request.Title, DateTime.UtcNow);
        await _repository.AddAsync(document, ct);

        return documentId;
    }
}
