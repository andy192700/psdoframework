using DoFramework.CLI;

namespace DoFramework.Processing;

public class EntryPoint : IEntryPoint
{
    private readonly IProcessDispatcher _dispatcher;
    private readonly IContext _context;
    private readonly CLIFunctionParameters _parameters;

    public EntryPoint(
        IProcessDispatcher dispatcher,
        IContext context,
        CLIFunctionParameters parameters)
    {
        _dispatcher = dispatcher;
        _context = context;
        _parameters = parameters;
    }

    public IContext? Enter()
    {
        return Enter(new ProcessingRequest([_parameters.Parameters!["name"].ToString()!], _parameters.Parameters));
    }

    public IContext? Enter(IProcessingRequest processingRequest)
    {
        _dispatcher.Dispatch(processingRequest);

        _context.Session.CurrentProcessName = string.Empty;

        return _parameters.ParseSwitch("doOutput") ? _context : null;
    }
}
