using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Documents.Relocation.Commands;

[PublicAPI]
public sealed record DeleteRelocationDocumentLineCommand(
    Guid DocumentId,
    Guid Id,
    long DocumentVersion
) : IRequest, ITxRequest;
