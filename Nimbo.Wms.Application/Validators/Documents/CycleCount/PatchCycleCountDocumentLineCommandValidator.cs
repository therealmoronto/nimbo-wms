using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Documents.CycleCount.Commands;

namespace Nimbo.Wms.Application.Validators.Documents.CycleCount;

[PublicAPI]
public class PatchCycleCountDocumentLineCommandValidator : AbstractValidator<PatchCycleCountDocumentLineCommand>
{
    public PatchCycleCountDocumentLineCommandValidator()
    {
        RuleFor(x => x.DocumentId)
            .NotEmpty()
            .WithMessage("Cycle count document is required");

        RuleFor(x => x.LineId)
            .NotEmpty()
            .WithMessage("Cycle count document line is required");

        RuleFor(x => x.ExpectedQuantity!.Value)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Expected quantity must be greater than or equal to 0")
            .When(x => x.ExpectedQuantity != null);

        RuleFor(x => x.DocumentVersion)
            .GreaterThan(0)
            .WithMessage("Document version must be greater than 0");
    }
}
