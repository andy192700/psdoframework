namespace DoFramework.Data;

/// <summary>
/// Defines a method to provide data based on a specified parameter.
/// </summary>
/// <typeparam name="TData">The type of data to provide.</typeparam>
/// <typeparam name="TParam">The type of the parameter used to provide the data.</typeparam>
public interface IDataProvider<TData, TParam>
{
    /// <summary>
    /// Provides data based on the specified parameter.
    /// </summary>
    /// <param name="parameter">The parameter used to provide the data.</param>
    /// <returns>The provided data.</returns>
    TData Provide(TParam parameter);
}

/// <summary>
/// Defines a method to provide a collection of data based on a specified parameter.
/// </summary>
/// <typeparam name="TData">The type of data in the collection to provide.</typeparam>
/// <typeparam name="TParam">The type of the parameter used to provide the data collection.</typeparam>
public interface IDataCollectionProvider<TData, TParam> : IDataProvider<List<TData>, TParam>
{

}

/// <summary>
/// Defines a method to provide data without any parameters.
/// </summary>
/// <typeparam name="TData">The type of data to provide.</typeparam>
public interface ISimpleDataProvider<TData>
{
    /// <summary>
    /// Provides data.
    /// </summary>
    /// <returns>The provided data.</returns>
    TData Provide();
}
