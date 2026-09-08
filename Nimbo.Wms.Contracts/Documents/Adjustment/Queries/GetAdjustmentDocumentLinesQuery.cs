using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Documents.Adjustment.Dtos;

namespace Nimbo.Wms.Contracts.Documents.Adjustment.Queries;

[PublicAPI]
public sealed record GetAdjustmentDocumentLinesQuery(Guid Id) : IRequest<Result<List<AdjustmentDocumentLineDto>>>;
