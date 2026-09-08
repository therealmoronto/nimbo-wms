using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Documents.CycleCount.Dtos;

namespace Nimbo.Wms.Contracts.Documents.CycleCount.Queries;

[PublicAPI]
public sealed record GetCycleCountDocumentQuery(Guid DocumentId) : IRequest<Result<CycleCountDocumentDto>>;
