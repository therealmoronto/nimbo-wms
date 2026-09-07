using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Documents.Adjustment.Commands;
using Nimbo.Wms.Domain.Entities.Documents;
using Nimbo.Wms.Domain.Entities.Documents.Adjustment;

namespace Nimbo.Wms.Application.Validators.Documents.Adjustment;

[PublicAPI]
public class PatchAdjustmentDocumentCommandValidator : AbstractValidator<PatchAdjustmentDocumentCommand>
{
    public PatchAdjustmentDocumentCommandValidator()
    {
        RuleFor(x => x.Code)
            .MaximumLength(IDocument.CodeMaxLength)
            .When(x => x.Code != null);

        RuleFor(x => x.Title)
            .MaximumLength(IDocument.TitleMaxLength)
            .When(x => x.Title != null);

        RuleFor(x => x.ReasonCode)
            .MaximumLength(AdjustmentDocument.ReasonCodeMaxLength)
            .When(x => x.ReasonCode != null);

        RuleFor(x => x.ReasonText)
            .MaximumLength(AdjustmentDocument.ReasonTextMaxLength)
            .When(x => x.ReasonText != null);

        RuleFor(x => x.Version)
            .GreaterThan(0)
            .WithMessage("Document version must be greater than 0");
    }
}
