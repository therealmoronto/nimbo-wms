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
            .MaximumLength(Supplier.CodeMaxLength);

        RuleFor(x => x.Name)
            .MaximumLength(Supplier.NameMaxLength);

        RuleFor(x => x.TaxId)
            .MaximumLength(Supplier.TaxIdMaxLength);

        RuleFor(x => x.Address)
            .MaximumLength(Supplier.AddressMaxLength);

        RuleFor(x => x.ContactName)
            .MaximumLength(Supplier.ContactNameMaxLength);

        RuleFor(x => x.Phone)
            .MaximumLength(Supplier.PhoneMaxLength);

        RuleFor(x => x.Email)
            .MaximumLength(Supplier.EmailMaxLength);
    }
}
