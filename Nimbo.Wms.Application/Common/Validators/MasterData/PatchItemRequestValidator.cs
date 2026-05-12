using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.Entities.MasterData;

namespace Nimbo.Wms.Application.Common.Validators.MasterData;

[PublicAPI]
public class PatchItemRequestValidator : AbstractValidator<PatchItemCommand>
{
    public PatchItemRequestValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(Item.NameMaxLength);

        RuleFor(x => x.InternalSku)
            .MaximumLength(Item.InternalSkuMaxLength);

        RuleFor(x => x.Barcode)
            .MaximumLength(Item.BarcodeMaxLength);

        RuleFor(x => x.Manufacturer)
            .MaximumLength(Item.ManufacturerMaxLength);
    }
}
