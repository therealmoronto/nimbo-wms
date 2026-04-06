using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Topology.Requests;
using Nimbo.Wms.Domain.Entities.Topology;

namespace Nimbo.Wms.Application.Common.Validators.Togology;

[PublicAPI]
public class CreateWarehouseRequestValidator : AbstractValidator<CreateWarehouseRequest>
{
    public CreateWarehouseRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(Warehouse.NameMaxLength);

        RuleFor(x => x.Code)
            .NotNull()
            .NotEmpty()
            .MaximumLength(Warehouse.CodeMaxLength);
    }
}
