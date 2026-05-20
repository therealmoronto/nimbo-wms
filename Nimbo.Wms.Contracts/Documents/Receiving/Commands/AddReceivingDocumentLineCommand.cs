using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Contracts.Documents.Receiving.Commands;

[PublicAPI]
public sealed record AddReceivingDocumentLineCommand(
    Guid DocumentId,
    Guid ItemId,
    Guid ToLocationId,
    QuantityDto ExpectedQuantity,
    string? Notes,
    long DocumentVersion
) : IRequest<Guid>, ITxRequest;
