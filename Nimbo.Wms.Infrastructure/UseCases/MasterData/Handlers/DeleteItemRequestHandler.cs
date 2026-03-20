using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

[PublicAPI]
internal sealed class DeleteItemRequestHandler : IRequestHandler<DeleteItemRequest>
{
    private readonly IItemRepository _repository;
    private readonly IUnitOfWork _uow;

    public DeleteItemRequestHandler(IItemRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task Handle(DeleteItemRequest request, CancellationToken ct = default)
    {
        var itemId = ItemId.From(request.ItemGuid);
        var item = await _repository.GetByIdAsync(itemId, ct);
        if (item is null)
            throw new NotFoundException($"Item with id {itemId} not found");

        await _repository.DeleteAsync(item, ct);
        await _uow.CommitAsync(ct);
    }
}
