using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Topology.Commands;
using Nimbo.Wms.Domain.Entities.Topology;

namespace Nimbo.Wms.Application.Common.Validators.Togology;

[PublicAPI]
public class AddLocationRequestValidator : AbstractValidator<AddLocationCommand>
{
    public AddLocationRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotNull()
            .NotEmpty()
            .MaximumLength(Location.CodeMaxLength)
            .WithMessage($"Location code cannot exceed {Location.CodeMaxLength} characters");
    }
}
