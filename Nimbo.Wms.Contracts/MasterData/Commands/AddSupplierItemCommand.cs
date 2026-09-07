using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.MasterData.Commands;

[PublicAPI]
public sealed record AddSupplierItemCommand(Guid SupplierGuid, Guid ItemGuid) : IRequest<Guid>, ITxRequest;
