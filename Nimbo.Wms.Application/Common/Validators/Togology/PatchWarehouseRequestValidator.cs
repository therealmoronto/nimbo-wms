using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Topology.Commands;
using Nimbo.Wms.Domain.Entities.Topology;

namespace Nimbo.Wms.Application.Common.Validators.Togology;

[PublicAPI]
public class PatchWarehouseRequestValidator : AbstractValidator<PatchWarehouseCommand>
{
    public PatchWarehouseRequestValidator()
    {
        RuleFor(x => x.Code)
            .MaximumLength(Warehouse.CodeMaxLength)
            .WithMessage($"Warehouse code cannot exceed {Warehouse.CodeMaxLength} characters");

        RuleFor(x => x.Name)
            .MaximumLength(Warehouse.NameMaxLength)
            .WithMessage($"Warehouse name cannot exceed {Warehouse.NameMaxLength} characters");

        RuleFor(x => x.Address)
            .MaximumLength(Warehouse.AddressMaxLength)
            .WithMessage($"Warehouse address cannot exceed {Warehouse.AddressMaxLength} characters");

        RuleFor(x => x.Description)
            .MaximumLength(Warehouse.DescriptionMaxLength)
            .WithMessage($"Warehouse description cannot exceed {Warehouse.DescriptionMaxLength} characters");
    }
}
