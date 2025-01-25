namespace DoFramework.FileSystem;

/// <summary>
/// Concrete implementation for <see cref="IOSSanitise"/> contract.
/// </summary>
public class OSSanitise : IOSSanitise
{
    /// <inheritdoc />
    public string Sanitise(string path)
    {
        var separator = Environment.Environment.Separator;

        return path.Replace('\\', separator).Replace('/', separator);
    }
}
