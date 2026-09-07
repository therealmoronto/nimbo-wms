using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Documents.Adjustment.Commands;

namespace Nimbo.Wms.Application.Validators.Documents.Adjustment;

[PublicAPI]
public class AddAdjustmentDocumentLineCommandValidator : AbstractValidator<AddAdjustmentDocumentLineCommand>
{
    public AddAdjustmentDocumentLineCommandValidator()
    {
        RuleFor(x => x.ItemId).NotEmpty();
        RuleFor(x => x.LocationId).NotEmpty();
        RuleFor(x => x.Delta).NotNull();
        RuleFor(x => x.DocumentVersion).GreaterThan(0);
    }
}
