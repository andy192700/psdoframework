using System.Reflection;

namespace PSDoFramework.Tool;

/// <summary>
/// POCO for injecting build time values.
/// </summary>
public class PowerShellSettings
{
    public string? Repository { get; set; } = "PSGallery";

    public string FrameworkVersion = GetVersion();

    private static string GetVersion()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version;

        var versionParts = version!.ToString().Split('.');

        return string.Join(".", versionParts[0], versionParts[1], versionParts[2]);
    }
}
