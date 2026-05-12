using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Stock.Requests;
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
            .MaximumLength(Batch.BatchNumberMaxLength);

        RuleFor(x => x.Notes)
            .MaximumLength(Batch.NotesMaxLength);
    }
}
