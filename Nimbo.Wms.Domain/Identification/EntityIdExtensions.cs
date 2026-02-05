namespace Nimbo.Wms.Domain.Identification;

/// <summary>
/// Strongly-typed entity identifier (wraps Guid).
/// No business rules besides non-empty.
/// </summary>
public static class EntityIdExtensions
{
    /// <summary>
    /// Creates a new entity identifier.
    /// </summary>
    /// <param name="ctor">Constructor for IEntityId implementation class</param>
    /// <typeparam name="T">IEntityId implementation class</typeparam>
    /// <returns></returns>
    public static T New<T>(Func<Guid, T> ctor) where T : struct, IEntityId
    {
        var guid = Guid.NewGuid();
        EnsureNotEmpty<T>(guid);
        return ctor(guid);
    }

    /// <summary>
    /// Creates an entity identifier from a Guid.
    /// </summary>
    /// <param name="guid">GUID which used to create IEntityId instance</param>
    /// <param name="ctor">Contructor for IEntityId</param>
    /// <typeparam name="T">IEntityId implementation class</typeparam>
    /// <returns>Created typed IEntityId instance</returns>
    /// <exception cref="ArgumentException">Thrown when GUID is empty</exception>
    public static T From<T>(Guid guid, Func<Guid, T> ctor) where T : struct, IEntityId
    {
        EnsureNotEmpty<T>(guid);
        return ctor(guid);
    }

    /// <summary>
    /// Throws an exception if entity identifier is empty.
    /// </summary>
    /// <param name="guid">IEntityId instance</param>
    /// <typeparam name="T">IEntityId implementation class</typeparam>
    /// <exception cref="ArgumentException">Thrown when IEntityId is empty</exception>
    public static void EnsureNotEmpty<T>(Guid guid) where T : struct, IEntityId
    {
        if (guid == Guid.Empty)
            throw new ArgumentException($"{typeof(T).Name} cannot be empty.");
    }
}
