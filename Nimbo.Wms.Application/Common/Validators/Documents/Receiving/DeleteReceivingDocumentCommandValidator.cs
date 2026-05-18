using FluentValidation;
using Nimbo.Wms.Contracts.Documents.Receiving.Commands;

namespace Nimbo.Wms.Application.Common.Validators.Documents.Receiving;

public class DeleteReceivingDocumentCommandValidator : AbstractValidator<DeleteReceivingDocumentCommand>
{
    public DeleteReceivingDocumentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Receiving document is required");

        RuleFor(x => x.Version)
            .GreaterThan(0)
            .WithMessage("Document version must be greater than 0");
    }
}
