using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Documents.Shipment.Commands;

namespace Nimbo.Wms.Application.Validators.Documents.Shipment;

[PublicAPI]
public class DeleteShipmentDocumentLineCommandValidator : AbstractValidator<DeleteShipmentDocumentLineCommand>
{
    public DeleteShipmentDocumentLineCommandValidator()
    {
        RuleFor(x => x.DocumentId)
            .NotEmpty()
            .WithMessage("Document ID is required");

        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Line ID is required");

        RuleFor(x => x.DocumentVersion)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Document version is required");
    }
}