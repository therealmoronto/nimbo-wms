using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

[PublicAPI]
internal sealed class AddSupplierItemCommandHandler : IRequestHandler<AddSupplierItemCommand, Guid>
{
    private readonly ISupplierRepository _repository;

    public AddSupplierItemCommandHandler(ISupplierRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(AddSupplierItemCommand command, CancellationToken ct = default)
    {
        var supplierId = SupplierId.From(command.SupplierGuid);
        var supplier = await _repository.GetByIdWithItemsAsync(supplierId, ct);
        if (supplier is null)
            throw new NotFoundException("Supplier not found");

        var supplierItemId = SupplierItemId.New();
        var itemId = new ItemId(command.ItemGuid);
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

        return supplierItemId;
    }
}
