using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.MasterData.Http;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Commands;

public sealed record PatchSupplierItemCommand(
    SupplierId SupplierId,
    SupplierItemId SupplierItemId,
    PatchSupplierItemRequest Request
) : ICommand;

public sealed class PatchSupplierItemHandler : ICommandHandler<PatchSupplierItemCommand>
{
    private readonly ISupplierRepository _repository;
    private readonly IUnitOfWork _uow;

    public PatchSupplierItemHandler(ISupplierRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }
    
    public async Task HandleAsync(PatchSupplierItemCommand command, CancellationToken ct = default)
    {
        var supplier = await _repository.GetByIdAsync(command.SupplierId, ct);
        if (supplier is null)
            throw new NotFoundException("Supplier not found");
        
        var item = supplier.Items.SingleOrDefault(i => i.Id == command.SupplierItemId);
        if (item is null)
            throw new NotFoundException("Supplier item not found");

        var request = command.Request;
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

        await _uow.SaveChangesAsync(ct);
    }
}
