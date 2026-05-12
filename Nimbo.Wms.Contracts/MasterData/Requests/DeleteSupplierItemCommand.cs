using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

[PublicAPI]
public sealed record DeleteSupplierItemCommand(Guid SupplierGuid, Guid SupplierItemIGuid) : IRequest, ITxRequest;
