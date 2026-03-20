using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.MasterData.Dtos;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

[PublicAPI]
public sealed record GetSuppliersRequest : IRequest<IReadOnlyList<SupplierDto>>;
