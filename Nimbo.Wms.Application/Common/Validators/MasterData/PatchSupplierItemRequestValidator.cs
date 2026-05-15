using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.MasterData.Commands;
using Nimbo.Wms.Domain.Entities.MasterData;

namespace Nimbo.Wms.Application.Common.Validators.MasterData;

[PublicAPI]
public class PatchSupplierItemRequestValidator : AbstractValidator<PatchSupplierItemCommand>
{
    public PatchSupplierItemRequestValidator()
    {
        RuleFor(x => x.SupplierSku)
            .MaximumLength(SupplierItem.SupplierSkuMaxLength)
            .WithMessage($"Supplier SKU cannot exceed {SupplierItem.SupplierSkuMaxLength} characters");

        RuleFor(x => x.SupplierBarcode)
            .MaximumLength(SupplierItem.SupplierBarcodeMaxLength)
            .WithMessage($"Supplier barcode cannot exceed {SupplierItem.SupplierBarcodeMaxLength} characters");

        RuleFor(x => x.PurchaseUomCode)
            .MaximumLength(SupplierItem.PurchaseUomCodeMaxLength)
            .WithMessage($"Purchase UOM code cannot exceed {SupplierItem.PurchaseUomCodeMaxLength} characters");
    }
}
