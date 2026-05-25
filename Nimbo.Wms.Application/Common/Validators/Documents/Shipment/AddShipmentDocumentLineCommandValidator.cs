using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Documents.Shipment.Commands;

namespace Nimbo.Wms.Application.Common.Validators.Documents.Shipment;

[PublicAPI]
public class AddShipmentDocumentLineCommandValidator : AbstractValidator<AddShipmentDocumentLineCommand>
{
    public AddShipmentDocumentLineCommandValidator()
    {
        RuleFor(x => x.DocumentId)
            .NotEmpty()
            .WithMessage("Document ID is required");

        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item is required");

        RuleFor(x => x.RequestedQuantity.Value)
            .GreaterThan(0)
            .WithMessage("Requested quantity must be greater than zero");

        RuleFor(x => x.DocumentVersion)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Document version is required");
    }
}