using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Documents.Relocation.Commands;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Relocation;

[PublicAPI]
public class PatchRelocationDocumentLineCommandHandler : IRequestHandler<PatchRelocationDocumentLineCommand>
{
    private readonly IRelocationDocumentRepository _repository;

    public PatchRelocationDocumentLineCommandHandler(IRelocationDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(PatchRelocationDocumentLineCommand request, CancellationToken ct)
    {
        var documentId = RelocationDocumentId.From(request.DocumentId);
        var document = await _repository.GetByIdWithLinesAsync(documentId, ct);
        if (document is null)
            throw new NotFoundException($"Relocation document with ID '{documentId}' not found");

        if (document.Version > request.DocumentVersion)
            throw new ConcurrencyException($"Document version mismatch. Expected: {document.Version}, Actual: {request.DocumentVersion}");

        if (request.Quantity is not null)
        {
            var value = request.Quantity.Value;
            var uom = Enum.Parse<UnitOfMeasure>(request.Quantity.Uom);

            var quantity = new Quantity(value, uom);
            document.ChangeLineRelocationQuantity(request.Id, quantity);
        }

        if (request.FromLocationId is not null)
        {
            document.ChangeLineFrom(request.Id, LocationId.From(request.FromLocationId.Value));
        }

        if (request.ToLocationId is not null)
        {
            document.ChangeLineTo(request.Id, LocationId.From(request.ToLocationId.Value));
        }

        var line = document.GetLine(request.Id);
        if (request.Notes is not null)
            line.ChangeNotes(request.Notes);
    }
}
