using FluentValidation;
using Nimbo.Wms.Contracts.Documents.Receiving.Commands;

namespace Nimbo.Wms.Application.Validators.Documents.Receiving;

public class PatchReceivingDocumentCommandValidator : AbstractValidator<PatchReceivingDocumentCommand>
{
    public PatchReceivingDocumentCommandValidator()
    {
        RuleFor(x => x.Version)
            .GreaterThan(0)
            .WithMessage("Document version must be greater than 0");
    }
}
