using MediatR;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

public sealed record DeleteSupplierItemRequest(Guid SupplierGuid, Guid SupplierItemIGuid) : IRequest;