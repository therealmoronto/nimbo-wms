using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.Adjustment.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Adjustment;

[PublicAPI]
public class PatchAdjustmentDocumentCommandHandler : IRequestHandler<PatchAdjustmentDocumentCommand, Result>
{
    private readonly IAdjustmentDocumentRepository _repository;

    public PatchAdjustmentDocumentCommandHandler(IAdjustmentDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(PatchAdjustmentDocumentCommand request, CancellationToken ct)
    {
        var documentId = AdjustmentDocumentId.From(request.Id);
        var document = await _repository.GetByIdAsync(documentId, ct);
        if (document is null)
            return Error.NotFound("document.notfound", $"Adjustment document with ID '{documentId}' not found.");

        if (document.Version > request.Version)
            return Error.Conflict("document.conflict", $"Document version mismatch. Expected: {document.Version}, Actual: {request.Version}");

        if (!string.IsNullOrWhiteSpace(request.Code))
            document.ChangeCode(request.Code);

        if (!string.IsNullOrWhiteSpace(request.Title))
            document.ChangeTitle(request.Title);

        if (!string.IsNullOrWhiteSpace(request.ReasonCode))
            document.ChangeReason(request.ReasonCode, request.ReasonText ?? document.ReasonText);

        return Result.Success();
    }
}
