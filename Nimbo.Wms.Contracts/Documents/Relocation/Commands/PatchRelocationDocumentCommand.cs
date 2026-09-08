using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Documents.Relocation.Commands;

[PublicAPI]
public sealed record PatchRelocationDocumentCommand(
    Guid Id,
    string? Code,
    string? Title,
    string? Notes,
    long Version
) : IRequest<Result>, ITxRequest;
