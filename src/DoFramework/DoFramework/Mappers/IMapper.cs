namespace DoFramework.Mappers;

/// <summary>
/// Interface for mapping an object from a source type to a destination type.
/// </summary>
/// <typeparam name="TSource">The source type.</typeparam>
/// <typeparam name="TDestination">The destination type.</typeparam>
public interface IMapper<TSource, TDestination>
{
    /// <summary>
    /// Maps the specified source object to the destination type.
    /// </summary>
    /// <param name="source">The source object.</param>
    /// <returns>The mapped object of the destination type.</returns>
    TDestination Map(TSource source);
}

