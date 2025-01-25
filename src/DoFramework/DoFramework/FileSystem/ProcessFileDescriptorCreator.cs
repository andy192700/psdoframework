using DoFramework.Domain;
using DoFramework.Environment;
using System.Text;

namespace DoFramework.FileSystem;

/// <summary>
/// Class for creating process descriptor files.
/// </summary>
/// <param name="environment">The environment instance.</param>
/// <param name="fileManager">The file manager instance.</param>
public class ProcessDescriptorFileCreator(IEnvironment environment, IFileManager fileManager)
    : DescriptorFileCreator<ProcessDescriptor>(environment, fileManager)
{
    /// <summary>
    /// Populates the definition of the process descriptor.
    /// </summary>
    /// <param name="sb">The StringBuilder instance.</param>
    /// <param name="descriptor">The process descriptor instance.</param>
    protected override void PopulateDefinition(StringBuilder sb, ProcessDescriptor descriptor)
    {
        sb.AppendLine("using namespace DoFramework.Processing;");
        sb.AppendLine();
        sb.AppendLine($"class {descriptor.Name} : Process {"{"}");
        sb.AppendLine("    [void] Run() {");
        sb.AppendLine("        # TODO: implement process");
        sb.AppendLine("    }");
        sb.AppendLine("}");
    }
}
