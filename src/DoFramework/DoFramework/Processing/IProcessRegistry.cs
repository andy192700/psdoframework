namespace DoFramework.Processing;

public interface IProcessRegistry
{
    void RegisterProcess(string processName);

    IProcessingRequest ToProcessRequest();
}
