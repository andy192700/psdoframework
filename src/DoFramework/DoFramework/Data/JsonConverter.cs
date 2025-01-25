using Newtonsoft.Json;

namespace DoFramework.Data;

/// <summary>
/// Provides methods to serialize and deserialize objects to and from JSON format using Newtonsoft.Json.
/// </summary>
public class JsonConverter : IJsonConverter
{
    /// <summary>
    /// Deserializes the specified JSON string to an object of type <typeparamref name="TReturn"/>.
    /// </summary>
    /// <typeparam name="TReturn">The type of the object to deserialize.</typeparam>
    /// <param name="value">The JSON string to deserialize.</param>
    /// <returns>The deserialized object of type <typeparamref name="TReturn"/>.</returns>
    public TReturn Deserialize<TReturn>(string value)
    {
        return JsonConvert.DeserializeObject<TReturn>(value)!;
    }

    /// <summary>
    /// Serializes the specified object to a JSON string with indented formatting.
    /// </summary>
    /// <param name="value">The object to serialize.</param>
    /// <returns>A JSON string representation of the object.</returns>
    public string Serialize(object value)
    {
        return JsonConvert.SerializeObject(value, Formatting.Indented);
    }
}
