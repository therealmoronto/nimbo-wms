using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Documents.Shipment.Commands;
using Nimbo.Wms.Domain.Entities.Documents.Common;

namespace Nimbo.Wms.Application.Common.Validators.Documents.Shipment;

[PublicAPI]
public class CreateShipmentDocumentCommandValidator : AbstractValidator<CreateShipmentDocumentCommand>
{
    public CreateShipmentDocumentCommandValidator()
    {
        RuleFor(x => x.WarehouseId)
            .NotEmpty()
            .WithMessage("Warehouse is required");

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
