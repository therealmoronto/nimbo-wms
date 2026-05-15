using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.MasterData.Commands;
using Nimbo.Wms.Domain.Entities.MasterData;

namespace Nimbo.Wms.Application.Common.Validators.MasterData;

[PublicAPI]
public class PatchItemRequestValidator : AbstractValidator<PatchItemCommand>
{
    public PatchItemRequestValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(Item.NameMaxLength)
            .WithMessage($"Item name cannot exceed {Item.NameMaxLength} characters");

        RuleFor(x => x.InternalSku)
            .MaximumLength(Item.InternalSkuMaxLength)
            .WithMessage($"Item internal SKU cannot exceed {Item.InternalSkuMaxLength} characters");

        RuleFor(x => x.Barcode)
            .MaximumLength(Item.BarcodeMaxLength)
            .WithMessage($"Item barcode cannot exceed {Item.BarcodeMaxLength} characters");

        RuleFor(x => x.Manufacturer)
            .MaximumLength(Item.ManufacturerMaxLength)
            .WithMessage($"Item manufacturer cannot exceed {Item.ManufacturerMaxLength} characters");
    }
}
