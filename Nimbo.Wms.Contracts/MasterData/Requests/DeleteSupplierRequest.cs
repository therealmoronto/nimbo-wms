using MediatR;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

public sealed record DeleteSupplierRequest(Guid SupplierGuid) : IRequest;