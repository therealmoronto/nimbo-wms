using FluentValidation;
using Nimbo.Wms.Contracts.Documents.Receiving.Commands;

namespace Nimbo.Wms.Application.Common.Validators.Documents.Receiving;

public class AddReceivingDocumentLineCommandValidator : AbstractValidator<AddReceivingDocumentLineCommand>
{
    public AddReceivingDocumentLineCommandValidator()
    {
        RuleFor(x => x.DocumentId)
            .NotEmpty()
            .WithMessage("Receiving document is required");

        RuleFor(x => x.ToLocationId)
            .NotEmpty()
            .WithMessage("To location is required");

        RuleFor(x => x.ExpectedQuantity.Value)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Expected quantity must be greater than or equal to 0");

        RuleFor(x => x.DocumentVersion)
            .GreaterThan(0)
            .WithMessage("Document version must be greater than 0");
    }
}
