using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.ValueObject;

namespace Nimbo.Wms.Contracts.Documents.Receiving.Commands;

[PublicAPI]
public sealed record AddReceivingDocumentLineCommand(
    Guid DocumentId,
    Guid ItemId,
    Guid ToLocationId,
    QuantityDto ExpectedQuantity,
    DateTime? ExpiryDate,
    string? BatchNumber,
    string? Notes,
    long DocumentVersion
) : IRequest<Guid>, ITxRequest;
