using MediatR;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;

public sealed record CreateWarehouseRequest(
    string Code,
    string Name
) : IRequest<WarehouseId>;

public sealed record CreateWarehouseResponse(Guid Id);
