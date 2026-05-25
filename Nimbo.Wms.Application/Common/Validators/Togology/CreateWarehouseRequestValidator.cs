using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Topology.Commands;
using Nimbo.Wms.Domain.Entities.Topology;

namespace Nimbo.Wms.Application.Common.Validators.Togology;

[PublicAPI]
public class CreateWarehouseRequestValidator : AbstractValidator<CreateWarehouseCommand>
{
    public CreateWarehouseRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(Warehouse.NameMaxLength)
            .WithMessage($"Warehouse name cannot exceed {Warehouse.NameMaxLength} characters");

        RuleFor(x => x.Code)
            .NotNull()
            .NotEmpty()
            .MaximumLength(Warehouse.CodeMaxLength)
            .WithMessage($"Warehouse code cannot exceed {Warehouse.CodeMaxLength} characters");
    }
}
