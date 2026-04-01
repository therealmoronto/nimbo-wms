using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Topology.Requests;
using Nimbo.Wms.Domain.Entities.Topology;

namespace Nimbo.Wms.Application.Common.Validators.Togology;

[PublicAPI]
public class AddZoneRequestValidator : AbstractValidator<AddZoneRequest>
{
    public AddZoneRequestValidator()
    {
        RuleFor(z => z.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(Zone.NameMaxLength);

        RuleFor(z => z.Code)
            .NotNull()
            .NotEmpty()
            .MaximumLength(Zone.CodeMaxLength);
    }
}
