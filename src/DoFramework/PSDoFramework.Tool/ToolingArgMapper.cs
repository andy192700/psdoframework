using DoFramework.Mappers;
using System.Diagnostics;
using System.Text;

namespace PSDoFramework.Tool;

/// <summary>
/// Concrete impl that maps application args to a <see cref="Process"/>.
/// </summary>
public class ToolingArgMapper : IMapper<string[], Process>
{
    /// <inheritdoc/>
    public Process Map(string[] args)
    {
        var cmd = new StringBuilder($"-command doing");

        for (int i = 0; i < args.Length; i++)
        {
            cmd.Append($" {args[i]}");
        }        

        var process = new Process();

        process.StartInfo.FileName = "pwsh";

        process.StartInfo.Arguments = cmd.ToString();
        
        process.StartInfo.RedirectStandardError = true;

        process.StartInfo.UseShellExecute = false;

        return process;
    }
}
