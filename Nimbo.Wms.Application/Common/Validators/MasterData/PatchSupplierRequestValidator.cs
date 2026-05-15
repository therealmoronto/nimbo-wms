using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.MasterData.Commands;
using Nimbo.Wms.Domain.Entities.MasterData;

namespace Nimbo.Wms.Application.Common.Validators.MasterData;

[PublicAPI]
public class PatchSupplierRequestValidator : AbstractValidator<PatchSupplierCommand>
{
    public PatchSupplierRequestValidator()
    {
        RuleFor(x => x.Code)
            .MaximumLength(Supplier.CodeMaxLength)
            .WithMessage($"Supplier code cannot exceed {Supplier.CodeMaxLength} characters");

        RuleFor(x => x.Name)
            .MaximumLength(Supplier.NameMaxLength)
            .WithMessage($"Supplier name cannot exceed {Supplier.NameMaxLength} characters");

        RuleFor(x => x.TaxId)
            .MaximumLength(Supplier.TaxIdMaxLength)
            .WithMessage($"Supplier tax ID cannot exceed {Supplier.TaxIdMaxLength} characters");

        RuleFor(x => x.Address)
            .MaximumLength(Supplier.AddressMaxLength)
            .WithMessage($"Supplier address cannot exceed {Supplier.AddressMaxLength} characters");

        RuleFor(x => x.ContactName)
            .MaximumLength(Supplier.ContactNameMaxLength)
            .WithMessage($"Supplier contact name cannot exceed {Supplier.ContactNameMaxLength} characters");

        RuleFor(x => x.Phone)
            .MaximumLength(Supplier.PhoneMaxLength)
            .WithMessage($"Supplier phone cannot exceed {Supplier.PhoneMaxLength} characters");

        RuleFor(x => x.Email)
            .MaximumLength(Supplier.EmailMaxLength)
            .WithMessage($"Supplier email cannot exceed {Supplier.EmailMaxLength} characters");
    }
}
