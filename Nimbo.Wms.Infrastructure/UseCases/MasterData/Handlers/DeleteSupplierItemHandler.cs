using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

public sealed class DeleteSupplierItemHandler : IRequestHandler<DeleteSupplierItemRequest>
{
    private readonly ISupplierRepository _repository;
    private readonly IUnitOfWork _uow;

    public DeleteSupplierItemHandler(ISupplierRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task Handle(DeleteSupplierItemRequest request, CancellationToken ct = default)
    {
        var supplierId = SupplierId.From(request.SupplierGuid);
        var supplier = await _repository.GetByIdAsync(supplierId, ct);
        if (supplier is null)
            throw new NotFoundException("Supplier not found");

        var supplierItemId = SupplierItemId.From(request.SupplierItemIGuid);
        if (!supplier.RemoveItem(supplierItemId))
            throw new NotFoundException("Supplier item not found");

        await _uow.CommitAsync(ct);
    }
}