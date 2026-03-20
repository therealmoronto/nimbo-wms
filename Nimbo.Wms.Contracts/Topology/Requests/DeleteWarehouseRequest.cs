using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Contracts.Topology.Requests;

[PublicAPI]
public sealed record DeleteWarehouseRequest(WarehouseId WarehouseId) : IRequest;
