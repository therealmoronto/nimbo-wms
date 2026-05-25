using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Documents.Relocation.Commands;

[PublicAPI]
public sealed record PatchRelocationDocumentCommand(
    Guid Id,
    string? Code,
    string? Title,
    string? Notes,
    long Version
) : IRequest, ITxRequest;
