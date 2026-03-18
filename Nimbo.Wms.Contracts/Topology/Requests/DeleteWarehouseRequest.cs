using MediatR;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;

public sealed record DeleteWarehouseRequest(WarehouseId WarehouseId) : IRequest;
