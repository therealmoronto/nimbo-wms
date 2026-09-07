using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Documents.CycleCount.Commands;

namespace Nimbo.Wms.Application.Validators.Documents.CycleCount;

[PublicAPI]
public class DeleteCycleCountDocumentLineCommandValidator : AbstractValidator<DeleteCycleCountDocumentLineCommand>
{
    public DeleteCycleCountDocumentLineCommandValidator()
    {
        RuleFor(x => x.DocumentId)
            .NotEmpty()
            .WithMessage("Cycle count document is required");

        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Cycle count document line is required");

        RuleFor(x => x.DocumentVersion)
            .GreaterThan(0)
            .WithMessage("Document version must be greater than 0");
    }
}
