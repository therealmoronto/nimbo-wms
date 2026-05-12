using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Topology.Requests;
using Nimbo.Wms.Domain.Entities.Topology;

namespace Nimbo.Wms.Application.Common.Validators.Togology;

[PublicAPI]
public class PatchLocationRequestValidator : AbstractValidator<PatchLocationCommand>
{
    public PatchLocationRequestValidator()
    {
        RuleFor(x => x.Code)
            .MaximumLength(Location.CodeMaxLength);

        RuleFor(x => x.Aisle)
            .MaximumLength(Location.AisleMaxLength);

        RuleFor(x => x.Rack)
            .MaximumLength(Location.RackMaxLength);

        RuleFor(x => x.Level)
            .MaximumLength(Location.LevelMaxLength);

        RuleFor(x => x.Position)
            .MaximumLength(Location.PositionMaxLength);
    }
}
