namespace DoFramework.Processing;

public interface IEntryPoint
{
    IContext? Enter();
    IContext? Enter(IProcessingRequest processingRequest);
}
