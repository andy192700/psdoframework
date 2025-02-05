using DoFramework.CLI;
using DoFramework.Logging;
using DoFramework.Validators;

namespace DoFramework.Processing;

/// <summary>
/// Implements the IProcessDispatcher interface to manage the dispatching of processing requests.
/// </summary>
public class ProcessDispatcher : IProcessDispatcher
{
    private readonly IContext _context;
    private readonly IProcessRunner _runner;
    private readonly IDisplayReports _displayReports;
    private readonly IValidator<IProcessingRequest> _processingRequestValidator;
    private readonly ILogger _logger;
    private readonly CLIFunctionParameters _cliFunctionParameters;

    public ProcessDispatcher(
        IContext context,
        IProcessRunner runner,
        IDisplayReports displayReports,
        IValidator<IProcessingRequest> processingRequestValidator,
        ILogger logger,
        CLIFunctionParameters cliFunctionParameters)
    {
        _context = context;
        _runner = runner;
        _displayReports = displayReports;
        _processingRequestValidator = processingRequestValidator;
        _logger = logger;
        _cliFunctionParameters = cliFunctionParameters;
    }

    /// <summary>
    /// Dispatches the specified processing request.
    /// </summary>
    /// <param name="processingRequest">The request to be processed.</param>
    public void Dispatch(IProcessingRequest processingRequest)
    {
        var result = _processingRequestValidator.Validate(processingRequest);

        foreach (var error in result.Errors)
        {
            _logger.LogError(error);
        }

        if (result.IsValid)
        {
            foreach (var process in processingRequest.Processes)
            {
                _runner.Run(process);
            }

            if (_context.Session.ProcessReports.Count == _context.Session.ProcessCount 
                && _cliFunctionParameters.ParseSwitch("showReports"))
			{
				_displayReports.Display(_context.Session.ProcessReports);
			}
        }
    }
}
