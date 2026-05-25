using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Documents.Adjustment.Commands;

namespace Nimbo.Wms.Application.Common.Validators.Documents.Adjustment;

[PublicAPI]
public class DeleteAdjustmentDocumentCommandValidator : AbstractValidator<DeleteAdjustmentDocumentCommand>
{
    public DeleteAdjustmentDocumentCommandValidator()
    {
        RuleFor(x => x.Version)
            .GreaterThan(0)
            .WithMessage("Document version must be greater than 0");
    }
}
