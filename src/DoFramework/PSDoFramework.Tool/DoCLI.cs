using DoFramework.Mappers;
using System.Diagnostics;

namespace PSDoFramework.Tool;

/// <summary>
/// Concrete implementation that drives communication with the Do framework's CLI
/// </summary>
public class DoCLI : IDoCLI
{
    private readonly IMapper<string[], Process> _mapper;

    public DoCLI(IMapper<string[], Process> mapper)
    {
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public void Exec(string[] args)
    {
        using var cmd = _mapper.Map(args);

        cmd.Start();

        cmd.WaitForExit();

        if (cmd.ExitCode != 0)
        {
            Console.WriteLine(cmd.StandardError.ReadToEnd());

            Environment.Exit(1);
        }
    }
}
