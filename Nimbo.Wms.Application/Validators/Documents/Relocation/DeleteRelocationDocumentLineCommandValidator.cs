using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Documents.Relocation.Commands;

namespace Nimbo.Wms.Application.Validators.Documents.Relocation;

[PublicAPI]
public class DeleteRelocationDocumentLineCommandValidator : AbstractValidator<DeleteRelocationDocumentLineCommand>
{
    public DeleteRelocationDocumentLineCommandValidator()
    {
        RuleFor(x => x.DocumentId)
            .NotEmpty()
            .WithMessage("Relocation document is required");

        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Relocation document line is required");

        RuleFor(x => x.DocumentVersion)
            .GreaterThan(0)
            .WithMessage("Document version must be greater than 0");
    }
}
