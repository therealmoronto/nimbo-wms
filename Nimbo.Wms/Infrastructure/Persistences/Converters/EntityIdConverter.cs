using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistences.Converters;

public class EntityIdConverter<TId> : ValueConverter<TId, Guid>
    where TId : struct, IEntityId
{
    /// <exception cref="InvalidOperationException">Thrown when the provided type TId does not implement IEntityId</exception>
    public EntityIdConverter()
        : base(v => v.Value, v => Create(v)) { }

    private static TId Create(Guid guid)
    {
        var value = Activator.CreateInstance(typeof(TId), guid);
        if (value == null)
            throw new InvalidOperationException($"Could not create an instance of {typeof(TId)}");

        return (TId)value;
    }
}
