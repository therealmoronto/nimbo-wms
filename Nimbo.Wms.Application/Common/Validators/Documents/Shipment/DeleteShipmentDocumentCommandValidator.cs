using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Documents.Shipment.Commands;

namespace Nimbo.Wms.Application.Common.Validators.Documents.Shipment;

[PublicAPI]
public class DeleteShipmentDocumentCommandValidator : AbstractValidator<DeleteShipmentDocumentCommand>
{
    public DeleteShipmentDocumentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Document ID is required");

        RuleFor(x => x.Version)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Document version is required");
    }
}