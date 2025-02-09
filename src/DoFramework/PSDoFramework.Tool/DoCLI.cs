using DoFramework.Mappers;
using System.Diagnostics;

namespace PSDoFramework.Tool;

public class DoCLI : IDoCLI
{
    private readonly IMapper<string[], Process> _mapper;

    public DoCLI(IMapper<string[], Process> mapper)
    {
        _mapper = mapper;
    }

    public void Exec(string[] args)
    {
        var cmd = _mapper.Map(args);
        cmd.Start();
        cmd.WaitForExit();
    }
}
