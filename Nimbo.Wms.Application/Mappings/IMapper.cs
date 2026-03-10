using JetBrains.Annotations;

namespace Nimbo.Wms.Application.Mappings;

[PublicAPI]
public interface IMapper<TMaster, TDto>
    where TMaster : class
    where TDto : class
{
    public IQueryable<TDto> ProjectToDto(IQueryable<TMaster> items);

    public IEnumerable<TDto> MapToDto(IEnumerable<TMaster> items);

    public TDto MapToDto(TMaster item);
}
