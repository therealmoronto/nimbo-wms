using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Documents.Shipment.Commands;

[PublicAPI]
public sealed record ChangeCustomerCommand(
    Guid DocumentId,
    Guid? CustomerId,
    long DocumentVersion
) : IRequest, ITxRequest;