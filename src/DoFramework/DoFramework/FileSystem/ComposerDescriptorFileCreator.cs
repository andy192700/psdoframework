using DoFramework.Domain;
using DoFramework.Environment;
using System.Text;

namespace DoFramework.FileSystem;

/// <summary>
/// Class for creating composer descriptor files.
/// </summary>
/// <param name="environment">The environment instance.</param>
/// <param name="fileManager">The file manager instance.</param>
public class ComposerDescriptorFileCreator(IEnvironment environment, IFileManager fileManager)
        : DescriptorFileCreator<ComposerDescriptor>(environment, fileManager)
{
    /// <inheritdoc/>
    protected override void PopulateDefinition(StringBuilder sb, ComposerDescriptor descriptor)
    {
        sb.AppendLine("using namespace DoFramework.Processing;");
        sb.AppendLine();
        sb.AppendLine($"class {descriptor.Name} : IComposer {"{"}");
        sb.AppendLine("    [void] Compose([IComposerWorkBench] $workBench) {");
        sb.AppendLine("        # TODO: implement composer");
        sb.AppendLine("    }");
        sb.AppendLine("}");
    }
}
