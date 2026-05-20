using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Contracts.Documents.Receiving.Commands;
using Nimbo.Wms.Domain.Entities.Documents.Receiving;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Receiving;

[PublicAPI]
public class CreateReceivingDocumentCommandHandler : IRequestHandler<CreateReceivingDocumentCommand, Guid>
{
    private readonly IReceivingDocumentRepository _repository;

    public CreateReceivingDocumentCommandHandler(IReceivingDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateReceivingDocumentCommand request, CancellationToken ct)
    {
        var warehouseId = WarehouseId.From(request.WarehouseId);
        var supplierId = SupplierId.From(request.SupplierId);
        var documentId = ReceivingDocumentId.New();

        var document = new ReceivingDocument(documentId, warehouseId, supplierId, request.Code, request.Title, DateTime.UtcNow, request.ExternalReference);
        await _repository.AddAsync(document, ct);

        return documentId;
    }
}
