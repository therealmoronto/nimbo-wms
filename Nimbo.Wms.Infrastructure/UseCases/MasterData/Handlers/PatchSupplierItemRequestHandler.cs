using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

[PublicAPI]
internal sealed class PatchSupplierItemRequestHandler : IRequestHandler<PatchSupplierItemRequest>
{
    private readonly ISupplierRepository _repository;

    public PatchSupplierItemRequestHandler(ISupplierRepository repository)
    {
        _repository = repository;
    }
    
    public async Task Handle(PatchSupplierItemRequest request, CancellationToken ct = default)
    {
        var supplierId = SupplierId.From(request.SupplierGuid);
        var supplier = await _repository.GetByIdWithItemsAsync(supplierId, ct);
        if (supplier is null)
            throw new NotFoundException("Supplier not found");

        var supplierItemId = SupplierItemId.From(request.SupplierItemGuid);
        var item = supplier.Items.SingleOrDefault(i => i.Id == supplierItemId);
        if (item is null)
            throw new NotFoundException("Supplier item not found");

        if (request.SupplierSku is not null)
            item.SetSupplierSku(request.SupplierSku);

        if (request.SupplierBarcode is not null)
            item.SetSupplierBarcode(request.SupplierBarcode);

        if (request.DefaultPurchasePrice is not null)
            item.SetDefaultPurchasePrice(request.DefaultPurchasePrice);

        if (request.PurchaseUomCode is not null || request.UnitsPerPurchaseUom is not null)
        {
            var purchaseUomCode = request.PurchaseUomCode ?? item.PurchaseUomCode;
            var unitsPerPurchaseUom = request.UnitsPerPurchaseUom ?? item.UnitsPerPurchaseUom;
            item.SetPurchaseUom(purchaseUomCode, unitsPerPurchaseUom);
        }
        
        if (request.LeadTimeDays is not null)
            item.SetLeadTimeDays(request.LeadTimeDays);
        
        if (request.MinOrderQty is not null)
            item.SetMinOrderQty(request.MinOrderQty);

        if (request.IsPreferred.HasValue)
        {
            if (request.IsPreferred.Value)
                item.MarkPreferred();
            else
                item.UnmarkPreferred();
        }
    }
}
