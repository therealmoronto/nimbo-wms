using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.MasterData.Commands;

[PublicAPI]
public sealed record CreateSupplierCommand(
    string Code,
    string Name
) : IRequest<Guid>, ITxRequest;
