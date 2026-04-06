using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

[PublicAPI]
public sealed record DeleteSupplierRequest(Guid SupplierGuid) : IRequest, ITxRequest;
