using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.Entities.MasterData;

namespace Nimbo.Wms.Application.Common.Validators.MasterData;

[PublicAPI]
public class PatchSupplierItemRequestValidator : AbstractValidator<PatchSupplierItemCommand>
{
    public PatchSupplierItemRequestValidator()
    {
        RuleFor(x => x.SupplierSku)
            .MaximumLength(SupplierItem.SupplierSkuMaxLength);

        RuleFor(x => x.SupplierBarcode)
            .MaximumLength(SupplierItem.SupplierBarcodeMaxLength);

        RuleFor(x => x.PurchaseUomCode)
            .MaximumLength(SupplierItem.PurchaseUomCodeMaxLength);
    }
}
