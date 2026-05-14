using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Documents.Receiving.Dtos;

namespace Nimbo.Wms.Contracts.Documents.Receiving.Queries;

[PublicAPI]
public sealed record GetReceivingDocumentLinesCommand(Guid Id) : IRequest<List<ReceivingDocumentLineDto>>;
