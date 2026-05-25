using FluentValidation;
using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Documents.Shipment.Commands;
using Nimbo.Wms.Domain.Entities.Documents.Common;

namespace Nimbo.Wms.Application.Common.Validators.Documents.Shipment;

[PublicAPI]
public class PatchShipmentDocumentCommandValidator : AbstractValidator<PatchShipmentDocumentCommand>
{
    public PatchShipmentDocumentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Document ID is required");

        RuleFor(x => x.Version)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Document version is required");

        When(x => !string.IsNullOrEmpty(x.Code), () =>
        {
            RuleFor(x => x.Code)
                .MaximumLength(IDocument.CodeMaxLength)
                .WithMessage("Document code exceeds the maximum length of {MaxLength} characters");
        });

        When(x => !string.IsNullOrEmpty(x.Title), () =>
        {
            RuleFor(x => x.Title)
                .MaximumLength(IDocument.TitleMaxLength)
                .WithMessage("Document title exceeds the maximum length of {MaxLength} characters");
        });
    }
}