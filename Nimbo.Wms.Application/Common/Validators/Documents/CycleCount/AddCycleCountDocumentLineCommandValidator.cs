using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Documents.CycleCount.Commands;

namespace Nimbo.Wms.Application.Common.Validators.Documents.CycleCount;

[PublicAPI]
public class AddCycleCountDocumentLineCommandValidator : AbstractValidator<AddCycleCountDocumentLineCommand>
{
    public AddCycleCountDocumentLineCommandValidator()
    {
        RuleFor(x => x.DocumentId)
            .NotEmpty()
            .WithMessage("Cycle count document is required");

        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item is required");

        RuleFor(x => x.LocationId)
            .NotEmpty()
            .WithMessage("Location is required");

        RuleFor(x => x.ExpectedQuantity.Value)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Expected quantity must be greater than or equal to 0");

        RuleFor(x => x.DocumentVersion)
            .GreaterThan(0)
            .WithMessage("Document version must be greater than 0");
    }
}
