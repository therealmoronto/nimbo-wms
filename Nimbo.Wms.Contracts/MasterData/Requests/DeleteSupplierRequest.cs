using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

[PublicAPI]
public sealed record DeleteSupplierRequest(Guid SupplierGuid) : IRequest;
