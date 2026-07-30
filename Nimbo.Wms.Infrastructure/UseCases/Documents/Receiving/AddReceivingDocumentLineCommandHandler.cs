using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Documents.Receiving.Commands;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Receiving;

[PublicAPI]
public class AddReceivingDocumentLineCommandHandler : IRequestHandler<AddReceivingDocumentLineCommand, Guid>
{
    private readonly IReceivingDocumentRepository _repository;
    private readonly IItemRepository _itemRepository;

    public AddReceivingDocumentLineCommandHandler(IReceivingDocumentRepository repository, IItemRepository itemRepository)
    {
        _repository = repository;
        _itemRepository = itemRepository;
    }

    public async Task<Guid> Handle(AddReceivingDocumentLineCommand request, CancellationToken ct)
    {
        var documentId = ReceivingDocumentId.From(request.DocumentId);
        var document = await _repository.GetByIdWithLinesAsync(documentId, ct);
        if (document is null)
            throw new NotFoundException($"Receiving document with ID '{documentId}' not found");

        if (document.Version > request.DocumentVersion)
            throw new ConcurrencyException($"Document version mismatch. Expected: {document.Version}, Actual: {request.DocumentVersion}");

        var itemId = ItemId.From(request.ItemId);
        var item = await _itemRepository.GetByIdAsync(itemId, ct);
        if (item is null)
            throw new InvalidOperationException($"Item with ID '{itemId}' not found");

        var toLocationId = LocationId.From(request.ToLocationId);
        var uom = Enum.Parse<UnitOfMeasure>(request.ExpectedQuantity.Uom);
        var expectedQuantity = new Quantity(request.ExpectedQuantity.Value, uom);

        return document.AddLine(itemId, Quantity.Zero(uom), toLocationId, expectedQuantity, request.ExpiryDate?.UtcDateTime, request.BatchNumber, request.Notes);
    }
}
