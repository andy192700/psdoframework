using static System.Net.Mime.MediaTypeNames;

namespace DoFramework.Mappers;

/// <summary>
/// Class for mapping an array of objects to a dictionary.
/// </summary>
public class ArgMapper : IMapper<object[], Dictionary<string, object>>
{
    /// <summary>
    /// Maps an array of objects to a dictionary.
    /// </summary>
    /// <param name="source">The source array of objects.</param>
    /// <returns>A dictionary with keys and values from the source array.</returns>
    public Dictionary<string, object> Map(object[] source)
    {
        var i = 0;

        var dictionary = new Dictionary<string, object>();

        while (i < source.Length)
        {
            if ((source[i].ToString()!.StartsWith("-")
                  && i == source.Length - 1))
            {
                dictionary[source[i].ToString()![1..]] = true;

                break;
            }

            var current = source[i].ToString()![1..];

            var next = source[i + 1];

            if (next.ToString()!.StartsWith("-"))
            {
                dictionary[current] = true;

                i += 1;
            }
            else
            {
                dictionary[current] = next;

                i += 2;
            }
        }

        return dictionary;
    }
}
