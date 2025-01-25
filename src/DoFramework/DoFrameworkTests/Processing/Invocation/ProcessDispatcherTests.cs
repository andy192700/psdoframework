using AutoFixture.Xunit2;
using DoFramework.CLI;
using DoFramework.Domain;
using DoFramework.Logging;
using DoFramework.Processing;
using DoFramework.Validators;
using Moq;

namespace DoFrameworkTests.Processing;

public class ProcessDispatcherTests
{
    [Theory]
    [InlineAutoMoqData]
    public void ProcessDispatcher_DispatchesProcessesNotComplete(
        [Frozen] Mock<IContext> context,
        [Frozen] Mock<IProcessRunner> runner,
        CLIFunctionParameters cliFunctionParameters,
        string[] processes)
    {
        // Arrange
        var displayReports = new Mock<IDisplayReports>();

        var processingRequestValidator = new Mock<IValidator<IProcessingRequest>>();
        processingRequestValidator.Setup(x => x.Validate(It.IsAny<IProcessingRequest>())).Returns(new ValidationResult([]));

        cliFunctionParameters.Parameters = new Dictionary<string, object>
        {
            { "showReports", false }
        };

        context.Setup(x => x.Session).Returns(new Session
        {
            ProcessReports = [],
            ProcessCount = processes.Length
        });

        var logger = new Mock<ILogger>();

        var contextWriter = new Mock<IContextWriter>();

        var sut = new ProcessDispatcher(
            context.Object,
            runner.Object,
            displayReports.Object,
            processingRequestValidator.Object,
            logger.Object,
            contextWriter.Object,
            cliFunctionParameters);

        var request = new ProcessingRequest(processes);

        // Act
        sut.Dispatch(request);

        // Assert
        processingRequestValidator.Verify(x => x.Validate(It.IsAny<IProcessingRequest>()), Times.Once);
        logger.Verify(x => x.LogError(It.IsAny<string>()), Times.Never);
        contextWriter.Verify(x => x.Write(It.IsAny<Dictionary<string, object>>()), Times.Once);
        runner.Verify(x => x.Run(It.IsAny<string>()), Times.Exactly(processes.Length));
        displayReports.Verify(x => x.Display(It.IsAny<List<ProcessReport>>()), Times.Never);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ProcessDispatcher_DispatchesProcessesComplete(
        [Frozen] Mock<IContext> context,
        [Frozen] Mock<IProcessRunner> runner,
        CLIFunctionParameters cliFunctionParameters,
        string[] processes)
    {
        // Arrange
        var displayReports = new Mock<IDisplayReports>();

        var processingRequestValidator = new Mock<IValidator<IProcessingRequest>>();
        processingRequestValidator.Setup(x => x.Validate(It.IsAny<IProcessingRequest>())).Returns(new ValidationResult([]));

        var reports = new List<ProcessReport>();

        foreach (var process in processes) 
        {
            reports.Add(new());
        }

        cliFunctionParameters.Parameters = new Dictionary<string, object>
        {
            { "showReports", true }
        };

        context.Setup(x => x.Session).Returns(new Session
        {
            ProcessReports = reports,
            ProcessCount = processes.Length
        });

        var logger = new Mock<ILogger>();

        var contextWriter = new Mock<IContextWriter>();

        var sut = new ProcessDispatcher(
            context.Object,
            runner.Object,
            displayReports.Object,
            processingRequestValidator.Object,
            logger.Object,
            contextWriter.Object,
            cliFunctionParameters);

        var request = new ProcessingRequest(processes);

        // Act
        sut.Dispatch(request);

        // Assert
        processingRequestValidator.Verify(x => x.Validate(It.IsAny<IProcessingRequest>()), Times.Once);
        logger.Verify(x => x.LogError(It.IsAny<string>()), Times.Never);
        contextWriter.Verify(x => x.Write(It.IsAny<Dictionary<string, object>>()), Times.Once);
        runner.Verify(x => x.Run(It.IsAny<string>()), Times.Exactly(processes.Length));
        displayReports.Verify(x => x.Display(It.IsAny<List<ProcessReport>>()), Times.Once);
    }

	[Theory]
	[InlineAutoMoqData(false, 0)]
	[InlineAutoMoqData(true, 1)]
	public void ProcessDispatcher_DispatchesProcessesCompletePerformOutputTests(
        bool showReports,
        int outputReportsCount,
		[Frozen] Mock<IContext> context,
		[Frozen] Mock<IProcessRunner> runner,
        CLIFunctionParameters cliFunctionParameters,
        string[] processes)
	{
		// Arrange
		var displayReports = new Mock<IDisplayReports>();

		var processingRequestValidator = new Mock<IValidator<IProcessingRequest>>();
		processingRequestValidator.Setup(x => x.Validate(It.IsAny<IProcessingRequest>())).Returns(new ValidationResult([]));

		var reports = new List<ProcessReport>();

		foreach (var process in processes)
		{
			reports.Add(new());
        }

        cliFunctionParameters.Parameters = new Dictionary<string, object>
        {
            { "showReports", showReports }
        };

        context.Setup(x => x.Session).Returns(new Session
		{
			ProcessReports = reports,
			ProcessCount = processes.Length
		});

		var logger = new Mock<ILogger>();

		var contextWriter = new Mock<IContextWriter>();

		var sut = new ProcessDispatcher(
			context.Object,
			runner.Object,
			displayReports.Object,
			processingRequestValidator.Object,
			logger.Object,
			contextWriter.Object, 
            cliFunctionParameters);

		var request = new ProcessingRequest(processes);

		// Act
		sut.Dispatch(request);

		// Assert
		processingRequestValidator.Verify(x => x.Validate(It.IsAny<IProcessingRequest>()), Times.Once);
		logger.Verify(x => x.LogError(It.IsAny<string>()), Times.Never);
		contextWriter.Verify(x => x.Write(It.IsAny<Dictionary<string, object>>()), Times.Once);
		runner.Verify(x => x.Run(It.IsAny<string>()), Times.Exactly(processes.Length));
		displayReports.Verify(x => x.Display(It.IsAny<List<ProcessReport>>()), Times.Exactly(outputReportsCount));
	}

	[Theory]
    [InlineAutoMoqData]
    public void ProcessDispatcher_DoesNotDispatchInvalid(
        [Frozen] Mock<IContext> context,
        [Frozen] Mock<IProcessRunner> runner,
        CLIFunctionParameters cliFunctionParameters,
        string[] processes,
        List<string> errors)
    {
        // Arrange
        var displayReports = new Mock<IDisplayReports>();

        var processingRequestValidator = new Mock<IValidator<IProcessingRequest>>();
        processingRequestValidator.Setup(x => x.Validate(It.IsAny<IProcessingRequest>())).Returns(new ValidationResult(errors));

        var reports = new List<ProcessReport>();

        foreach (var process in processes)
        {
            reports.Add(new());
		}

        cliFunctionParameters.Parameters = new Dictionary<string, object>
        {
            { "showReports", false }
        };

        context.Setup(x => x.Session).Returns(new Session
        {
            ProcessReports = reports,
            ProcessCount = processes.Length
        });

        var logger = new Mock<ILogger>();

        var contextWriter = new Mock<IContextWriter>();

        var sut = new ProcessDispatcher(
            context.Object,
            runner.Object,
            displayReports.Object,
            processingRequestValidator.Object,
            logger.Object,
            contextWriter.Object,
            cliFunctionParameters);

        var request = new ProcessingRequest(processes);

        // Act
        sut.Dispatch(request);

        // Assert
        processingRequestValidator.Verify(x => x.Validate(It.IsAny<IProcessingRequest>()), Times.Once);
        logger.Verify(x => x.LogError(It.IsAny<string>()), Times.Exactly(errors.Count));
        contextWriter.Verify(x => x.Write(It.IsAny<Dictionary<string, object>>()), Times.Never);
        runner.Verify(x => x.Run(It.IsAny<string>()), Times.Never);
        displayReports.Verify(x => x.Display(It.IsAny<List<ProcessReport>>()), Times.Never);
    }
}