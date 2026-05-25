using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Documents.Relocation.Commands;
using Nimbo.Wms.Domain.Entities.Documents.Common;

namespace Nimbo.Wms.Application.Common.Validators.Documents.Relocation;

[PublicAPI]
public class PatchRelocationDocumentCommandValidator : AbstractValidator<PatchRelocationDocumentCommand>
{
    public PatchRelocationDocumentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Relocation document is required");

        RuleFor(x => x.Code)
            .MaximumLength(IDocument.CodeMaxLength)
            .WithMessage("Document code exceeds the maximum length of {MaxLength} characters");

        RuleFor(x => x.Title)
            .MaximumLength(IDocument.TitleMaxLength)
            .WithMessage("Document title exceeds the maximum length of {MaxLength} characters");

        RuleFor(x => x.Version)
            .GreaterThan(0)
            .WithMessage("Document version must be greater than 0");
    }
}
