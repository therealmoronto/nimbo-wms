using FluentValidation;
using Nimbo.Wms.Contracts.Documents.Receiving.Commands;

namespace Nimbo.Wms.Application.Validators.Documents.Receiving;

public class DeleteReceivingDocumentLineCommandValidator : AbstractValidator<DeleteReceivingDocumentLineCommand>
{
    public DeleteReceivingDocumentLineCommandValidator()
    {
        RuleFor(x => x.DocumentId)
            .NotEmpty()
            .WithMessage("Receiving document is required");

        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Receiving document line is required");

        RuleFor(x => x.DocumentVersion)
            .GreaterThan(0)
            .WithMessage("Document version must be greater than 0");
    }
}
