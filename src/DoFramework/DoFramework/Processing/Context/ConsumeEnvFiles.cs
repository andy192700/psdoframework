using DoFramework.Data;

namespace DoFramework.Processing;

/// <summary>
/// Class for consuming environment files and writing their contents to the context.
/// </summary>
/// <param name="contextWriter">The context writer instance.</param>
/// <param name="envFileDataProvider">The environment file data provider.</param>
public class ConsumeEnvFiles(IContextWriter contextWriter, ISimpleDataProvider<Dictionary<string, object>> envFileDataProvider) : IConsumeEnvFiles
{
    private readonly IContextWriter _contextWriter = contextWriter;

    private readonly ISimpleDataProvider<Dictionary<string, object>> _envFileDataProvider = envFileDataProvider;

    /// <summary>
    /// Consumes the environment files by reading their data and writing it to the context.
    /// </summary>
    public void Consume()
    {
        _contextWriter.Write(_envFileDataProvider!.Provide());
    }
}
