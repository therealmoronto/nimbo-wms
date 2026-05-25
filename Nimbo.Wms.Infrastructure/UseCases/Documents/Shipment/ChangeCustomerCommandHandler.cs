using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Documents.Shipment.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Shipment;

[PublicAPI]
public class ChangeCustomerCommandHandler : IRequestHandler<ChangeCustomerCommand>
{
    private readonly IShipmentDocumentRepository _repository;

    public ChangeCustomerCommandHandler(IShipmentDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(ChangeCustomerCommand request, CancellationToken ct)
    {
        var documentId = ShipmentDocumentId.From(request.DocumentId);
        var document = await _repository.GetByIdAsync(documentId, ct);
        if (document is null)
            throw new InvalidOperationException($"Shipment document with ID {request.DocumentId} not found");

        if (document.Version > request.DocumentVersion)
            throw new ConcurrencyException($"Document version mismatch. Expected: {document.Version}, Actual: {request.DocumentVersion}");

        if (request.CustomerId is not null)
        {
            var customerId = CustomerId.From(request.CustomerId.Value);
            document.ChangeCustomer(customerId);
        }
    }
}