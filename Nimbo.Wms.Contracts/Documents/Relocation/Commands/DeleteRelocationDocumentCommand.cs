using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Documents.Relocation.Commands;

[PublicAPI]
public record DeleteRelocationDocumentCommand(Guid Id, long Version) : IRequest, ITxRequest;
