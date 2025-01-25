namespace DoFramework.Data;

/// <summary>
/// Provides methods to serialize and deserialize objects to and from JSON format.
/// </summary>
public interface IJsonConverter
{
    /// <summary>
    /// Serializes the specified object to a JSON string.
    /// </summary>
    /// <param name="value">The object to serialize.</param>
    /// <returns>A JSON string representation of the object.</returns>
    string Serialize(object value);

    /// <summary>
    /// Deserializes the specified JSON string to an object of type <typeparamref name="TReturn"/>.
    /// </summary>
    /// <typeparam name="TReturn">The type of the object to deserialize.</typeparam>
    /// <param name="value">The JSON string to deserialize.</param>
    /// <returns>The deserialized object of type <typeparamref name="TReturn"/>.</returns>
    TReturn Deserialize<TReturn>(string value);
}
