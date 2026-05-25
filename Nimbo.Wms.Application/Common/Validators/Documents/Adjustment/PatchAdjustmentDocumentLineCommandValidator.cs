using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Documents.Adjustment.Commands;

namespace Nimbo.Wms.Application.Common.Validators.Documents.Adjustment;

[PublicAPI]
public class PatchAdjustmentDocumentLineCommandValidator : AbstractValidator<PatchAdjustmentDocumentLineCommand>
{
    public PatchAdjustmentDocumentLineCommandValidator()
    {
        RuleFor(x => x.DocumentVersion).GreaterThan(0);
    }
}
