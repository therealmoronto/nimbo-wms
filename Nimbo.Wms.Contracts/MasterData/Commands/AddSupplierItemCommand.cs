using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.MasterData.Commands;

[PublicAPI]
public sealed record AddSupplierItemCommand(Guid SupplierGuid, Guid ItemGuid) : IRequest<Guid>, ITxRequest;
