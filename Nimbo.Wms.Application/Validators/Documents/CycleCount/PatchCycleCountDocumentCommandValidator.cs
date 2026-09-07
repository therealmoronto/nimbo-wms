using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Documents.CycleCount.Commands;
using Nimbo.Wms.Domain.Entities.Documents;

namespace Nimbo.Wms.Application.Validators.Documents.CycleCount;

[PublicAPI]
public class PatchCycleCountDocumentCommandValidator : AbstractValidator<PatchCycleCountDocumentCommand>
{
    public PatchCycleCountDocumentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Cycle count document is required");

        RuleFor(x => x.Code)
            .MaximumLength(IDocument.CodeMaxLength)
            .WithMessage("Document code exceeds the maximum length of {MaxLength} characters")
            .When(x => x.Code != null);

        RuleFor(x => x.Title)
            .MaximumLength(IDocument.TitleMaxLength)
            .WithMessage("Document title exceeds the maximum length of {MaxLength} characters")
            .When(x => x.Title != null);

        RuleFor(x => x.Version)
            .GreaterThan(0)
            .WithMessage("Document version must be greater than 0");
    }
}
