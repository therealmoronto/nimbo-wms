using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.MasterData.Http;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Commands;

public sealed record AddSupplierItemCommand(
    AddSupplierItemRequest Request
) : ICommand<SupplierItemId>;

public sealed class AddSupplierItemHandler : ICommandHandler<AddSupplierItemCommand, SupplierItemId>
{
    private readonly ISupplierRepository _repository;
    private readonly IUnitOfWork _uow;

    public AddSupplierItemHandler(ISupplierRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task<SupplierItemId> HandleAsync(AddSupplierItemCommand command, CancellationToken ct = default)
    {
        var request = command.Request;
        var supplierId = SupplierId.From(request.SupplierId);
        var supplier = await _repository.GetByIdWithItemsAsync(supplierId, ct);
        if (supplier is null)
            throw new NotFoundException("Supplier not found");
        
        var supplierItemId = SupplierItemId.New();
        var itemId = new ItemId(request.ItemId);
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

        await _uow.SaveChangesAsync(ct);
        return supplierItemId;
    }
}
