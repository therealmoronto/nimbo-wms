using MediatR;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

public sealed record DeleteItemRequest(Guid ItemGuid) : IRequest;