using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Documents.Adjustment.Commands;

namespace Nimbo.Wms.Application.Validators.Documents.Adjustment;

[PublicAPI]
public class DeleteAdjustmentDocumentLineCommandValidator : AbstractValidator<DeleteAdjustmentDocumentLineCommand>
{
    public DeleteAdjustmentDocumentLineCommandValidator()
    {
        RuleFor(x => x.DocumentVersion).GreaterThan(0);
    }
}
