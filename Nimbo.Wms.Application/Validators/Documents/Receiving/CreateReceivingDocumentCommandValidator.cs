using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Documents.Receiving.Commands;
using Nimbo.Wms.Domain.Entities.Documents;

namespace Nimbo.Wms.Application.Validators.Documents.Receiving;

[PublicAPI]
public class CreateReceivingDocumentCommandValidator : AbstractValidator<CreateReceivingDocumentCommand>
{
    public CreateReceivingDocumentCommandValidator()
    {
        RuleFor(x => x.WarehouseId)
            .NotEmpty()
            .WithMessage("Warehouse is required");

        RuleFor(x => x.SupplierId)
            .NotEmpty()
            .WithMessage("Supplier is required");

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(IDocument.CodeMaxLength)
            .WithMessage("Document code is required or exceeds the maximum length of {MaxLength} characters");

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(IDocument.TitleMaxLength)
            .WithMessage("Document title is required or exceeds the maximum length of {MaxLength} characters");
    }
}
