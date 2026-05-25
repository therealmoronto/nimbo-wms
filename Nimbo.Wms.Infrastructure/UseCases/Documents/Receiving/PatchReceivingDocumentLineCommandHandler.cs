using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Documents.Receiving.Commands;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Receiving;

[PublicAPI]
public class PatchReceivingDocumentLineCommandHandler : IRequestHandler<PatchReceivingDocumentLineCommand>
{
    private readonly IReceivingDocumentRepository _repository;

    public PatchReceivingDocumentLineCommandHandler(IReceivingDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(PatchReceivingDocumentLineCommand request, CancellationToken ct)
    {
        var documentId = ReceivingDocumentId.From(request.DocumentId);
        var document = await _repository.GetByIdWithLinesAsync(documentId, ct);
        if (document is null)
            throw new NotFoundException($"Receiving document with ID '{documentId}' not found");

        if (document.Version > request.DocumentVersion)
            throw new ConcurrencyException($"Document version mismatch. Expected: {document.Version}, Actual: {request.DocumentVersion}");

        if (request.ExpectedQuantity is not null)
        {
            var value = request.ExpectedQuantity.Value;
            var uom = Enum.Parse<UnitOfMeasure>(request.ExpectedQuantity.Uom);

            var quantity = new Quantity(value, uom);
            document.ChangeLineExpectedQuantity(request.Id, quantity);
        }

        if (request.Notes is not null)
            document.ChangeLineNotes(request.Id, request.Notes);
    }
}
