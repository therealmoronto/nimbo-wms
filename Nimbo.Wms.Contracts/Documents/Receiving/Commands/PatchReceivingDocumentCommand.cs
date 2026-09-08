using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Documents.Receiving.Commands;

[PublicAPI]
public sealed record PatchReceivingDocumentCommand(
    Guid Id,
    string? Code,
    string? Title,
    string? Notes,
    long Version
) : IRequest<Result>, ITxRequest;
