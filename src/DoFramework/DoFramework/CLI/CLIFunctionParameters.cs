namespace DoFramework.CLI;

/// <summary>
/// Represents the parameters for a CLI function.
/// </summary>
public class CLIFunctionParameters
{
    /// <summary>
    /// Gets or sets the parameters for the CLI function.
    /// </summary>
    public Dictionary<string, object>? Parameters { get; set; }

    /// <summary>
    /// Parses a switch, returning a bool matching the key, if it exists. False always if it does not
    /// </summary>
    /// <param name="key">The name of the key to parse.</param>
    /// <returns>THe parsed switch.</returns>
    public bool ParseSwitch(string key)
    {        
        if (Parameters is null)
        {
            return false;
        }

        if (Parameters!.ContainsKey(key))
        {
            if (bool.TryParse(Parameters![key].ToString(), out var switchValue) && switchValue)
            {
                return true;
            }

            return false;
        }

        return false;
    }
}
