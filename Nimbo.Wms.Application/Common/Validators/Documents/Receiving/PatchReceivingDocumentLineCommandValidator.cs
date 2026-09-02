using FluentValidation;
using Nimbo.Wms.Contracts.Documents.Receiving.Commands;

namespace Nimbo.Wms.Application.Common.Validators.Documents.Receiving;

public class PatchReceivingDocumentLineCommandValidator : AbstractValidator<PatchReceivingDocumentLineCommand>
{
    public PatchReceivingDocumentLineCommandValidator()
    {
        RuleFor(x => x.DocumentId)
            .NotEmpty()
            .WithMessage("Receiving document is required");

        RuleFor(x => x.ExpectedQuantity.Value)
            .GreaterThanOrEqualTo(0)
            .When(x => x.ExpectedQuantity != null)
            .WithMessage("Expected quantity must be non-negative");

        RuleFor(x => x.DocumentVersion)
            .GreaterThan(0)
            .WithMessage("Document version must be greater than 0");
    }
}
