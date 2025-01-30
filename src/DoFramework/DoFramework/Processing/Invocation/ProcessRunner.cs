using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.Logging;

namespace DoFramework.Processing;

/// <summary>
/// Implements the IProcessRunner interface to manage the execution of processes.
/// </summary>
public class ProcessRunner : IProcessRunner
{
    private readonly IContext _context;
    private readonly IResolver<ProcessDescriptor> _processResolver;
    private readonly IProcessExecutor _executor;
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessRunner"/> class.
    /// </summary>
    /// <param name="context">The context for the process execution.</param>
    /// <param name="processResolver">The resolver to obtain the process descriptor.</param>
    /// <param name="executor">The executor to run the process.</param>
    /// <param name="logger">The logger for logging errors.</param>
    /// <param name="computeHierarchyPrefix">The service to compute the hierarchy prefix.</param>
    public ProcessRunner(
        IContext context,
        IResolver<ProcessDescriptor> processResolver,
        IProcessExecutor executor,
        ILogger logger)
    {
        _context = context;
        _processResolver = processResolver;
        _executor = executor;
        _logger = logger;
    }

    /// <summary>
    /// Runs the specified task.
    /// </summary>
    /// <param name="task">The task to be executed.</param>
    public void Run(string task)
    {
        var result = _processResolver.Resolve(task);

        var descriptor = result.Descriptor;

        _context.Session.CurrentProcessName = task;

        _context.Session.ProcessCount += 1;

        ProcessReport report;

        if (descriptor != null)
        {
            report = _executor.Execute(descriptor);
        }
        else
        {
            report = new()
            {
                ProcessResult = ProcessResult.NotFound,
                Descriptor = new ProcessDescriptor
                {
                    Name = task
                }
            };

            _logger.LogFatal($"Process not found: {task}");
        }

        report.Name = $"{report.Descriptor!.Name}";

        _context.Session.ProcessReports.Add(report);
    }
}
