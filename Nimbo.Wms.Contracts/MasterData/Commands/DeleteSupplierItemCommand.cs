using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.MasterData.Commands;

[PublicAPI]
public sealed record DeleteSupplierItemCommand(Guid SupplierGuid, Guid SupplierItemIGuid) : IRequest, ITxRequest;
