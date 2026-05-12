using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

[PublicAPI]
internal sealed class PatchSupplierItemCommandHandler : IRequestHandler<PatchSupplierItemCommand>
{
    private readonly ISupplierRepository _repository;

    public PatchSupplierItemCommandHandler(ISupplierRepository repository)
    {
        _repository = repository;
    }
    
    public async Task Handle(PatchSupplierItemCommand command, CancellationToken ct = default)
    {
        var supplierId = SupplierId.From(command.SupplierGuid);
        var supplier = await _repository.GetByIdWithItemsAsync(supplierId, ct);
        if (supplier is null)
            throw new NotFoundException("Supplier not found");

        var supplierItemId = SupplierItemId.From(command.SupplierItemGuid);
        var item = supplier.Items.SingleOrDefault(i => i.Id == supplierItemId);
        if (item is null)
            throw new NotFoundException("Supplier item not found");

        if (command.SupplierSku is not null)
            item.SetSupplierSku(command.SupplierSku);

        if (command.SupplierBarcode is not null)
            item.SetSupplierBarcode(command.SupplierBarcode);

        if (command.DefaultPurchasePrice is not null)
            item.SetDefaultPurchasePrice(command.DefaultPurchasePrice);

        if (command.PurchaseUomCode is not null || command.UnitsPerPurchaseUom is not null)
        {
            var purchaseUomCode = command.PurchaseUomCode ?? item.PurchaseUomCode;
            var unitsPerPurchaseUom = command.UnitsPerPurchaseUom ?? item.UnitsPerPurchaseUom;
            item.SetPurchaseUom(purchaseUomCode, unitsPerPurchaseUom);
        }
        
        if (command.LeadTimeDays is not null)
            item.SetLeadTimeDays(command.LeadTimeDays);
        
        if (command.MinOrderQty is not null)
            item.SetMinOrderQty(command.MinOrderQty);

        if (command.IsPreferred.HasValue)
        {
            if (command.IsPreferred.Value)
                item.MarkPreferred();
            else
                item.UnmarkPreferred();
        }
    }
}
