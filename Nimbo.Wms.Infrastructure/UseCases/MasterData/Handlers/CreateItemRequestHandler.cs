using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

[PublicAPI]
internal sealed class CreateItemRequestHandler : IRequestHandler<CreateItemRequest, ItemId>
{
    private readonly IItemRepository _repository;
    private readonly IUnitOfWork _uow;

    public CreateItemRequestHandler(IItemRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task<ItemId> Handle(CreateItemRequest request, CancellationToken ct = default)
    {
        var item = new Item(
            ItemId.New(),
            request.Name,
            request.InternalSku,
            request.Barcode,
            request.BaseUom);

        await _repository.AddAsync(item, ct);
        await _uow.CommitAsync(ct);

        return item.Id;
    }
}
