using DoFramework.CLI;
using DoFramework.Logging;
using DoFramework.Validators;

namespace DoFramework.Processing;

public class EntryPoint : IEntryPoint
{
    private readonly IContext _context;
    private readonly IProcessRunner _runner;
    private readonly IDisplayReports _displayReports;
    private readonly IValidator<IProcessingRequest> _processingRequestValidator;
    private readonly ILogger _logger;
    private readonly CLIFunctionParameters _parameters;

    public EntryPoint(
        IContext context,
        IProcessRunner runner,
        IDisplayReports displayReports,
        IValidator<IProcessingRequest> processingRequestValidator,
        ILogger logger,
        CLIFunctionParameters parameters)
    {
        _context = context;
        _runner = runner;
        _displayReports = displayReports;
        _processingRequestValidator = processingRequestValidator;
        _logger = logger;
        _parameters = parameters;
    }

    public IContext? Enter()
    {
        return Enter(new ProcessingRequest([_parameters.Parameters!["name"].ToString()!]));
    }

    public IContext? Enter(IProcessingRequest processingRequest)
    {
        var dispatcher = new ProcessDispatcher(
            _context, 
            _runner, 
            _displayReports, 
            _processingRequestValidator, 
            _logger, 
            _parameters);

        dispatcher.Dispatch(processingRequest);

        _context.Session.CurrentProcessName = string.Empty;

        return _parameters.ParseSwitch("doOutput") ? _context : null;
    }
}
