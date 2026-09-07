using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Documents.Shipment.Commands;

[PublicAPI]
public sealed record ChangeCustomerCommand(
    Guid DocumentId,
    Guid? CustomerId,
    long DocumentVersion
) : IRequest, ITxRequest;