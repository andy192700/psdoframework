using DoFramework.Domain;
using DoFramework.Environment;
using System.Text;

namespace DoFramework.FileSystem;

/// <summary>
/// Class for creating module descriptor files.
/// </summary>
/// <param name="environment">The environment instance.</param>
/// <param name="fileManager">The file manager instance.</param>
public class ModuleDescriptorFileCreator(IEnvironment environment, IFileManager fileManager)
        : DescriptorFileCreator<ModuleDescriptor>(environment, fileManager)
{
    /// <summary>
    /// Populates the definition of the module descriptor.
    /// </summary>
    /// <param name="sb">The StringBuilder instance.</param>
    /// <param name="descriptor">The module descriptor instance.</param>
    protected override void PopulateDefinition(StringBuilder sb, ModuleDescriptor descriptor)
    {
        sb.AppendLine("# TODO: Create classes and functions.");
        sb.AppendLine();
        sb.AppendLine("function ExampleFunction {");
        sb.AppendLine("}");
        sb.AppendLine();
        sb.AppendLine("Export-ModuleMember -Function ExampleFunction;");
        sb.AppendLine();
        sb.AppendLine("class ExampleClass {");
        sb.AppendLine("}");
    }
}
