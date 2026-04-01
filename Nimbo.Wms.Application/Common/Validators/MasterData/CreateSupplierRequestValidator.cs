using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.Entities.MasterData;

namespace Nimbo.Wms.Application.Common.Validators.MasterData;

[PublicAPI]
public class CreateSupplierRequestValidator : AbstractValidator<CreateSupplierRequest>
{
    public CreateSupplierRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotNull()
            .NotEmpty()
            .MaximumLength(Supplier.CodeMaxLength);

        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(Supplier.NameMaxLength);
    }
}
