using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.MasterData.Commands;
using Nimbo.Wms.Domain.Entities.MasterData;

namespace Nimbo.Wms.Application.Common.Validators.MasterData;

[PublicAPI]
public class CreateItemRequestValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(Item.NameMaxLength);

        RuleFor(x => x.InternalSku)
            .NotNull()
            .NotEmpty()
            .MaximumLength(Item.InternalSkuMaxLength);

        RuleFor(x => x.Barcode)
            .NotNull()
            .NotEmpty()
            .MaximumLength(Item.BarcodeMaxLength);
    }
}
