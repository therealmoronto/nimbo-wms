using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Topology.Requests;
using Nimbo.Wms.Domain.Entities.Topology;

namespace Nimbo.Wms.Application.Common.Validators.Togology;

[PublicAPI]
public class PatchZoneRequestValidator : AbstractValidator<PatchZoneCommand>
{
    public PatchZoneRequestValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(Zone.NameMaxLength);

        RuleFor(x => x.Code)
            .MaximumLength(Zone.CodeMaxLength);
    }
}
