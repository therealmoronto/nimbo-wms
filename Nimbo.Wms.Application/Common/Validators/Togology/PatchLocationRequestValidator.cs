using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Topology.Commands;
using Nimbo.Wms.Domain.Entities.Topology;

namespace Nimbo.Wms.Application.Common.Validators.Togology;

[PublicAPI]
public class PatchLocationRequestValidator : AbstractValidator<PatchLocationCommand>
{
    public PatchLocationRequestValidator()
    {
        RuleFor(x => x.Code)
            .MaximumLength(Location.CodeMaxLength)
            .WithMessage($"Location code cannot exceed {Location.CodeMaxLength} characters");

        RuleFor(x => x.Aisle)
            .MaximumLength(Location.AisleMaxLength)
            .WithMessage($"Aisle cannot exceed {Location.AisleMaxLength} characters");

        RuleFor(x => x.Rack)
            .MaximumLength(Location.RackMaxLength)
            .WithMessage($"Rack cannot exceed {Location.RackMaxLength} characters");

        RuleFor(x => x.Level)
            .MaximumLength(Location.LevelMaxLength)
            .WithMessage($"Level cannot exceed {Location.LevelMaxLength} characters");

        RuleFor(x => x.Position)
            .MaximumLength(Location.PositionMaxLength)
            .WithMessage($"Position cannot exceed {Location.PositionMaxLength} characters");
    }
}
