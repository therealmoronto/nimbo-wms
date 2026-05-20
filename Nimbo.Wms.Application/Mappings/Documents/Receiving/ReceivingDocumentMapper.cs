using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Documents.Receiving.Dtos;
using Nimbo.Wms.Domain.Entities.Documents.Receiving;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.Documents.Receiving;

[PublicAPI]
[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public partial class ReceivingDocumentBodyMapper : IMapper<ReceivingDocument, ReceivingDocumentBodyDto>
{
    public partial IQueryable<ReceivingDocumentBodyDto> ProjectToDto(IQueryable<ReceivingDocument> items);

    public partial IEnumerable<ReceivingDocumentBodyDto> MapToDto(IEnumerable<ReceivingDocument> items);

    public partial ReceivingDocumentBodyDto MapToDto(ReceivingDocument item);
}

[PublicAPI]
[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public partial class ReceivingDocumentMapper : IMapper<ReceivingDocument, ReceivingDocumentDto>
{
    private readonly IMapper<ReceivingDocument, ReceivingDocumentBodyDto> _bodyMapper;
    private readonly IMapper<ReceivingDocumentLine, ReceivingDocumentLineDto> _lineMapper;

    public ReceivingDocumentMapper(
        IMapper<ReceivingDocument, ReceivingDocumentBodyDto> bodyMapper,
        IMapper<ReceivingDocumentLine, ReceivingDocumentLineDto> lineMapper)
    {
        _bodyMapper = bodyMapper;
        _lineMapper = lineMapper;
    }

    public IQueryable<ReceivingDocumentDto> ProjectToDto(IQueryable<ReceivingDocument> items) => items.Select(i => MapToDto(i));

    public IEnumerable<ReceivingDocumentDto> MapToDto(IEnumerable<ReceivingDocument> items) => items.Select(MapToDto);

    public ReceivingDocumentDto MapToDto(ReceivingDocument item)
    {
        return new ReceivingDocumentDto(_bodyMapper.MapToDto(item), _lineMapper.MapToDto(item.Lines).ToList());
    }
}
