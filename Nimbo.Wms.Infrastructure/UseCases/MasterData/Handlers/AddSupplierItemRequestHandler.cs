using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

public sealed class AddSupplierItemRequestHandler : IRequestHandler<AddSupplierItemRequest, SupplierItemId>
{
    private readonly ISupplierRepository _repository;
    private readonly IUnitOfWork _uow;

    public AddSupplierItemRequestHandler(ISupplierRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task<SupplierItemId> Handle(AddSupplierItemRequest request, CancellationToken ct = default)
    {
        var supplierId = SupplierId.From(request.SupplierGuid);
        var supplier = await _repository.GetByIdWithItemsAsync(supplierId, ct);
        if (supplier is null)
            throw new NotFoundException("Supplier not found");

        var supplierItemId = SupplierItemId.New();
        var itemId = new ItemId(request.ItemGuid);
        supplier.AddItem(
            supplierItemId,
            itemId,
            supplierSku: null,
            supplierBarcode: null,
            defaultPurchasePrice: null,
            purchaseUomCode: null,
            unitsPerPurchaseUom: null,
            leadTimeDays: null,
            minOrderQty: null,
            isPreferred: false);

        await _uow.CommitAsync(ct);
        return supplierItemId;
    }
}
