using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Topology.Commands;
using Nimbo.Wms.Domain.Entities.Topology;

namespace Nimbo.Wms.Application.Common.Validators.Togology;

[PublicAPI]
public class AddZoneRequestValidator : AbstractValidator<AddZoneCommand>
{
    public AddZoneRequestValidator()
    {
        RuleFor(z => z.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(Zone.NameMaxLength)
            .WithMessage($"Zone name cannot exceed {Zone.NameMaxLength} characters");

        RuleFor(z => z.Code)
            .NotNull()
            .NotEmpty()
            .MaximumLength(Zone.CodeMaxLength)
            .WithMessage($"Zone code cannot exceed {Zone.CodeMaxLength} characters");
    }
}
