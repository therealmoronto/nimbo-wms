using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Documents.CycleCount.Commands;

namespace Nimbo.Wms.Application.Common.Validators.Documents.CycleCount;

[PublicAPI]
public class DeleteCycleCountDocumentCommandValidator : AbstractValidator<DeleteCycleCountDocumentCommand>
{
    public DeleteCycleCountDocumentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Cycle count document is required");

        RuleFor(x => x.Version)
            .GreaterThan(0)
            .WithMessage("Document version must be greater than 0");
    }
}
