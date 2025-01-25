namespace DoFramework.Services;

/// <summary>
/// Implementation of the IObjectBuilder interface for building objects.
/// </summary>
public class ObjectBuilder : IObjectBuilder
{
    /// <summary>
    /// Builds an object of the specified type using the provided constructor parameters.
    /// </summary>
    /// <param name="type">The type of object to build.</param>
    /// <param name="constructorParams">The parameters to pass to the object's constructor.</param>
    /// <returns>An instance of the specified type.</returns>
    public object BuildObject(Type type, List<object> constructorParams)
    {
        return Activator.CreateInstance(type, constructorParams.ToArray())!;
    }
}
