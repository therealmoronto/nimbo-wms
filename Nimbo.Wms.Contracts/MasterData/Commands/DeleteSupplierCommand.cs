using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.MasterData.Commands;

[PublicAPI]
public sealed record DeleteSupplierCommand(Guid SupplierGuid) : IRequest<Result>, ITxRequest;
