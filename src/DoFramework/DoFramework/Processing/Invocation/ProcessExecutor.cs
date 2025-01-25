using DoFramework.Domain;
using DoFramework.Logging;

namespace DoFramework.Processing;

/// <summary>
/// Implements the IProcessExecutor interface to manage the execution of processes.
/// </summary>
public class ProcessExecutor : IProcessExecutor
{
    private readonly IContext _context;
    private readonly IProcessInstanceRunner _processInstanceRunner;
    private readonly IProcessBuilder _processBuilder;
    private readonly ILogger _logger;
    private readonly IFailedReportChecker _failedReportChecker;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessExecutor"/> class.
    /// </summary>
    /// <param name="context">The context for the process execution.</param>
    /// <param name="processInstanceRunner">The instance runner for the process.</param>
    /// <param name="processBuilder">The builder for the process.</param>
    /// <param name="logger">The logger for logging errors and warnings.</param>
    /// <param name="failedReportChecker">The checker for failed reports.</param>
    public ProcessExecutor(
        IContext context,
        IProcessInstanceRunner processInstanceRunner,
        IProcessBuilder processBuilder,
        ILogger logger,
        IFailedReportChecker failedReportChecker)
    {
        _context = context;
        _processInstanceRunner = processInstanceRunner;
        _processBuilder = processBuilder;
        _logger = logger;
        _failedReportChecker = failedReportChecker;
    }

    /// <summary>
    /// Executes a process based on the given descriptor and returns a process report.
    /// </summary>
    /// <param name="descriptor">The descriptor that specifies the process to execute.</param>
    /// <returns>A report of the executed process.</returns>
    public ProcessReport Execute(ProcessDescriptor descriptor)
    {
        descriptor.Instance = _processBuilder.Build(descriptor);

        var report = new ProcessReport
        {
            Descriptor = descriptor,
            ProcessResult = ProcessResult.NotRun
        };

        if (_failedReportChecker.Check())
        {
            _logger.LogWarning($"Process not run: {report.Descriptor.Name}");
        }
        else
        {
            bool valid;
            try
            {
                if (descriptor.Instance is null)
                {
                    valid = false;
                    _logger.LogFatal($"Could not build Process class: [{report.Descriptor!.Name}]");
                    _logger.LogFatal($"Process failed: {report.Descriptor.Name}");
                }
                else
                {
                    valid = descriptor.Instance!.Validate();
                }
            }
            catch (Exception ex)
            {
                valid = false;
                _logger.LogFatal($"Whilst validating {report.Descriptor!.Name}, an error occurred: {ex.Message}");
                _logger.LogFatal($"Process failed: {report.Descriptor.Name}");
            }

            if (valid)
            {
                _processInstanceRunner.RunInstance(report);
            }
            else
            {
                report.ProcessResult = ProcessResult.Invalidated;
                _logger.LogFatal($"Process invalidated: {report.Descriptor.Name}");
            }
        }

        if (_context.Session.CurrentProcessName != report.Descriptor.Name && _failedReportChecker.Check())
        {
            report.ProcessResult = ProcessResult.Failed;
        }

        return report;
    }
}
