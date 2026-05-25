using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Documents.Shipment.Commands;

namespace Nimbo.Wms.Application.Common.Validators.Documents.Shipment;

[PublicAPI]
public class AddPickLineCommandValidator : AbstractValidator<AddPickLineCommand>
{
    public AddPickLineCommandValidator()
    {
        RuleFor(x => x.DocumentId)
            .NotEmpty()
            .WithMessage("Document ID is required");

        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item is required");

        RuleFor(x => x.FromLocationId)
            .NotEmpty()
            .WithMessage("From Location is required");

        RuleFor(x => x.DocumentVersion)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Document version is required");
    }
}