using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Contracts.Documents.CycleCount.Commands;

[PublicAPI]
public sealed record AddCycleCountDocumentLineCommand(
    Guid DocumentId,
    Guid ItemId,
    Guid LocationId,
    QuantityDto ExpectedQuantity,
    string? Notes,
    long DocumentVersion
) : IRequest<Guid>, ITxRequest;
