using DoFramework.Domain;
using DoFramework.Environment;
using DoFramework.FileSystem;
using DoFramework.Logging;

namespace DoFramework.Processing;

/// <summary>
/// Implements the IProcessInstanceRunner interface to manage the execution of process instances.
/// </summary>
public class ProcessInstanceRunner : IProcessInstanceRunner
{
    private readonly IEnvironment _environment;
    private readonly ISetProcessLocation _setProcessLocation;
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessInstanceRunner"/> class.
    /// </summary>
    /// <param name="environment">The environment in which the process instance runs.</param>
    /// <param name="setProcessLocation">The service to set the process location.</param>
    /// <param name="logger">The logger for logging errors.</param>
    public ProcessInstanceRunner(
        IEnvironment environment,
        ISetProcessLocation setProcessLocation,
        ILogger logger)
    {
        _environment = environment;
        _setProcessLocation = setProcessLocation;
        _logger = logger;
    }

    /// <summary>
    /// Runs the process instance based on the given report.
    /// </summary>
    /// <param name="report">The report containing details about the process instance to run.</param>
    public void RunInstance(ProcessReport report)
    {
        try
        {
            report.StartTime = DateTime.Now;

            report.Descriptor!.Instance!.Run();

            report.ProcessResult = ProcessResult.Completed;
        }
        catch (Exception ex)
        {
            report.ProcessResult = ProcessResult.Failed;

            _logger.LogFatal($"Whilst executing {report.Descriptor!.Name}, an error occurred: {ex.Message}");

            _logger.LogFatal($"Process failed: {report.Descriptor.Name}");
        }
        finally
        {
            report.EndTime = DateTime.Now;

            _setProcessLocation.Set(_environment.HomeDir);
        }
    }
}
