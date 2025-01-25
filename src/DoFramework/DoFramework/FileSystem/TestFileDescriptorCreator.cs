using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.Environment;
using DoFramework.Testing;
using System.Text;

namespace DoFramework.FileSystem;

/// <summary>
/// Class for creating test descriptor files.
/// </summary>
/// <param name="environment">The environment instance.</param>
/// <param name="fileManager">The file manager instance.</param>
/// <param name="moduleProvider">The data collection provider for module descriptors.</param>
public class TestDescriptorFileCreator(
    IEnvironment environment,
    IFileManager fileManager,
    IDataCollectionProvider<ModuleDescriptor, string> moduleProvider)
        : DescriptorFileCreator<TestDescriptor>(environment, fileManager)
{
    private readonly IDataCollectionProvider<ModuleDescriptor, string> _moduleProvider = moduleProvider;

    /// <summary>
    /// Populates the definition of the test descriptor.
    /// </summary>
    /// <param name="sb">The StringBuilder instance.</param>
    /// <param name="descriptor">The test descriptor instance.</param>
    protected override void PopulateDefinition(StringBuilder sb, TestDescriptor descriptor)
    {
        ModuleDescriptor? module = default;

        if (descriptor.TestType == TestType.Module)
        {
            var modules = _moduleProvider.Provide(".*");

            module = modules.Find(x => x.Name == descriptor.Name!.Replace("Tests", string.Empty));
        }

        if (module != null)
        {
            sb.AppendLine($"using module \"{CreateRelativePathExtension(module.Path!)}..\\..\\Modules\\{module.Path}\";");
            sb.AppendLine();
        }

        sb.AppendLine("# TODO: Write tests");
        sb.AppendLine();
        sb.AppendLine($"Describe '{descriptor.Name}' {"{"}");
        sb.AppendLine("    BeforeEach {");
        sb.AppendLine("    }");
        sb.AppendLine("    Context 'Example' {");
        sb.AppendLine("        It 'Will Pass' {");
        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine("}");
    }

    /// <summary>
    /// Creates a relative path extension for the specified path.
    /// </summary>
    /// <param name="path">The original path.</param>
    /// <returns>The relative path extension as a string.</returns>
    private string CreateRelativePathExtension(string path)
    {
        var sep = Environment.Environment.Separator.ToString();
        var occurrences = path.Length - path.Replace(sep, string.Empty).Length;
        var sb = new StringBuilder();
        for (var i = 0; i < occurrences; i++)
        {
            sb.Append("..\\");
        }
        return sb.ToString();
    }
}
