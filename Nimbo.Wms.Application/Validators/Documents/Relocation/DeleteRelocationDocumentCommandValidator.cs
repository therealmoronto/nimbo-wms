using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Documents.Relocation.Commands;

namespace Nimbo.Wms.Application.Validators.Documents.Relocation;

[PublicAPI]
public class DeleteRelocationDocumentCommandValidator : AbstractValidator<DeleteRelocationDocumentCommand>
{
    public DeleteRelocationDocumentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Relocation document is required");

        RuleFor(x => x.Version)
            .GreaterThan(0)
            .WithMessage("Document version must be greater than 0");
    }
}
