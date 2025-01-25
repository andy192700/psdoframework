namespace DoFramework.Services;

/// <summary>
/// Interface for building objects of a specified type.
/// </summary>
public interface IObjectBuilder
{
    /// <summary>
    /// Builds an object of the specified type using the provided constructor parameters.
    /// </summary>
    /// <param name="type">The type of object to build.</param>
    /// <param name="constructorParams">The parameters to pass to the object's constructor.</param>
    /// <returns>An instance of the specified type.</returns>
    object BuildObject(Type type, List<object> constructorParams);
}
