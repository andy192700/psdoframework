namespace DoFramework.Data;

/// <summary>
/// Defines a method to delete an item of a specified type.
/// </summary>
/// <typeparam name="T">The type of item to delete.</typeparam>
public interface IDataDeletor<T>
{
    /// <summary>
    /// Deletes an item of the specified type.
    /// </summary>
    /// <param name="item">The item to delete.</param>
    void Delete(T item);
}
