using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Topology.Requests;
using Nimbo.Wms.Domain.Entities.Topology;

namespace Nimbo.Wms.Application.Common.Validators.Togology;

[PublicAPI]
public class PatchWarehouseRequestValidator : AbstractValidator<PatchWarehouseCommand>
{
    public PatchWarehouseRequestValidator()
    {
        RuleFor(x => x.Code)
            .MaximumLength(Warehouse.CodeMaxLength);

        RuleFor(x => x.Name)
            .MaximumLength(Warehouse.NameMaxLength);

        RuleFor(x => x.Address)
            .MaximumLength(Warehouse.AddressMaxLength);

        RuleFor(x => x.Description)
            .MaximumLength(Warehouse.DescriptionMaxLength);
    }
}
