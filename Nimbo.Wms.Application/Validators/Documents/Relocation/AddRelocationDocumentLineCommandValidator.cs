using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Documents.Relocation.Commands;

namespace Nimbo.Wms.Application.Validators.Documents.Relocation;

[PublicAPI]
public class AddRelocationDocumentLineCommandValidator : AbstractValidator<AddRelocationDocumentLineCommand>
{
    public AddRelocationDocumentLineCommandValidator()
    {
        RuleFor(x => x.DocumentId)
            .NotEmpty()
            .WithMessage("Relocation document is required");

        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item is required");

        RuleFor(x => x.FromLocationId)
            .NotEmpty()
            .WithMessage("From location is required");

        RuleFor(x => x.ToLocationId)
            .NotEmpty()
            .WithMessage("To location is required");

        RuleFor(x => x.Quantity.Value)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");

        RuleFor(x => x.DocumentVersion)
            .GreaterThan(0)
            .WithMessage("Document version must be greater than 0");
    }
}
