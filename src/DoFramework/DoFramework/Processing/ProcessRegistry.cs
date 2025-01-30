namespace DoFramework.Processing;

public class ProcessRegistry : IProcessRegistry
{
    private IList<string> Processes {  get; set; } = new List<string>();

    public void RegisterProcess(string processName)
    {
        Processes.Add(processName);
    }

    public IProcessingRequest ToProcessRequest()
    {
        return new ProcessingRequest([.. Processes]);
    }
}
