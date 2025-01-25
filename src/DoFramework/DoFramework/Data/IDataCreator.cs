namespace DoFramework.Data;

/// <summary>
/// Defines a method to create an item of a specified type.
/// </summary>
/// <typeparam name="T">The type of item to create.</typeparam>
public interface IDataCreator<T>
{
    /// <summary>
    /// Creates an item of the specified type.
    /// </summary>
    /// <param name="item">The item to create.</param>
    void Create(T item);
}
