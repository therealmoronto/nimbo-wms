using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Documents.Relocation.Dtos;

namespace Nimbo.Wms.Contracts.Documents.Relocation.Queries;

[PublicAPI]
public sealed record GetRelocationDocumentLinesQuery(Guid Id) : IRequest<List<RelocationDocumentLineDto>>;
