using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Stock.Commands;
using Nimbo.Wms.Domain.Entities.Stock;

namespace Nimbo.Wms.Application.Common.Validators.Stock;

[PublicAPI]
public class CreateInventoryItemRequestValidator : AbstractValidator<CreateInventoryItemCommand>
{
    public CreateInventoryItemRequestValidator()
    {
        RuleFor(x => x.SerialNumber)
            .MaximumLength(InventoryItem.SerialNumberMaxLength)
            .WithMessage($"Serial number cannot exceed {InventoryItem.SerialNumberMaxLength} characters");

        RuleFor(x => x.Quantity)
            .NotNull()
            .WithMessage("Quantity is required");
    }
}
