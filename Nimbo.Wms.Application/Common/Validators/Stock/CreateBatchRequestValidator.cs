using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Stock.Commands;
using Nimbo.Wms.Domain.Entities.Stock;

namespace Nimbo.Wms.Application.Common.Validators.Stock;

[PublicAPI]
public class CreateBatchRequestValidator : AbstractValidator<CreateBatchCommand>
{
    public CreateBatchRequestValidator()
    {
        RuleFor(x => x.BatchNumber)
            .NotNull()
            .NotEmpty()
            .MaximumLength(Batch.BatchNumberMaxLength)
            .WithMessage($"Batch number cannot exceed {Batch.BatchNumberMaxLength} characters");

        RuleFor(x => x.Notes)
            .MaximumLength(Batch.NotesMaxLength)
            .WithMessage($"Batch notes cannot exceed {Batch.NotesMaxLength} characters");
    }
}
