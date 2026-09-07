using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Documents.Shipment.Commands;

namespace Nimbo.Wms.Application.Validators.Documents.Shipment;

[PublicAPI]
public class RemovePickLineCommandValidator : AbstractValidator<RemovePickLineCommand>
{
    public RemovePickLineCommandValidator()
    {
        RuleFor(x => x.DocumentId)
            .NotEmpty()
            .WithMessage("Document ID is required");

        RuleFor(x => x.PickLineId)
            .NotEmpty()
            .WithMessage("Pick Line ID is required");

        RuleFor(x => x.DocumentVersion)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Document version is required");
    }
}