using DoFramework.Mappers;
using System.Diagnostics;

var mapper = new ToolingArgMapper();

var cmd = mapper.Map(args);

var process = new Process();
process.StartInfo.FileName = "pwsh";
process.StartInfo.Arguments = $"-command {cmd}";
process.Start();
process.WaitForExit();
