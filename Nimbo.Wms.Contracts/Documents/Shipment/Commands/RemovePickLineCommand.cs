using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Documents.Shipment.Commands;

[PublicAPI]
public sealed record RemovePickLineCommand(
    Guid DocumentId,
    Guid PickLineId,
    long DocumentVersion
) : IRequest, ITxRequest;