using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.Receiving.Commands;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Receiving;

[PublicAPI]
public class AddReceivingDocumentLineCommandHandler : IRequestHandler<AddReceivingDocumentLineCommand, Result<Guid>>
{
    private readonly IReceivingDocumentRepository _repository;
    private readonly IItemRepository _itemRepository;

    public AddReceivingDocumentLineCommandHandler(IReceivingDocumentRepository repository, IItemRepository itemRepository)
    {
        _repository = repository;
        _itemRepository = itemRepository;
    }

    public async Task<Result<Guid>> Handle(AddReceivingDocumentLineCommand request, CancellationToken ct)
    {
        var documentId = ReceivingDocumentId.From(request.DocumentId);
        var document = await _repository.GetByIdWithLinesAsync(documentId, ct);
        if (document is null)
            return Error.NotFound("document.notfound", $"Receiving document with ID '{documentId}' not found");

        if (document.Version > request.DocumentVersion)
            return Error.Conflict("document.conflict", $"Document version mismatch. Expected: {document.Version}, Actual: {request.DocumentVersion}");

        var itemId = ItemId.From(request.ItemId);
        var item = await _itemRepository.GetByIdAsync(itemId, ct);
        if (item is null)
            return Error.NotFound("item.notfound", $"Item with ID '{itemId}' not found");

        var toLocationId = LocationId.From(request.ToLocationId);
        var uom = Enum.Parse<UnitOfMeasure>(request.ExpectedQuantity.Uom);
        var expectedQuantity = new Quantity(request.ExpectedQuantity.Value, uom);

        // BatchNumber/ExpiryDate are carried as-is here — they're resolved into a real VendorLot only
        // at posting time (ReceivingDocumentPostingService), not validated at line-add time.
        return document.AddLine(itemId, Quantity.Zero(uom), toLocationId, expectedQuantity, request.ExpiryDate, request.BatchNumber, request.Notes);
    }
}
