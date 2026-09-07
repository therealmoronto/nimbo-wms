using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Documents.Adjustment.Commands;
using Nimbo.Wms.Domain.Entities.Documents;
using Nimbo.Wms.Domain.Entities.Documents.Adjustment;

namespace Nimbo.Wms.Application.Validators.Documents.Adjustment;

[PublicAPI]
public class CreateAdjustmentDocumentCommandValidator : AbstractValidator<CreateAdjustmentDocumentCommand>
{
    public CreateAdjustmentDocumentCommandValidator()
    {
        RuleFor(x => x.WarehouseId)
            .NotEmpty()
            .WithMessage("Warehouse is required");

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(IDocument.CodeMaxLength)
            .WithMessage("Document code is required or exceeds the maximum length of {MaxLength} characters");

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(IDocument.TitleMaxLength)
            .WithMessage("Document title is required or exceeds the maximum length of {MaxLength} characters");

        RuleFor(x => x.ReasonCode)
            .NotEmpty()
            .MaximumLength(AdjustmentDocument.ReasonCodeMaxLength)
            .WithMessage("Reason code is required or exceeds the maximum length of {MaxLength} characters");

        RuleFor(x => x.ReasonText)
            .MaximumLength(AdjustmentDocument.ReasonTextMaxLength)
            .WithMessage("Reason text exceeds the maximum length of {MaxLength} characters");
    }
}
