using AutoFixture.Xunit2;
using DoFramework.Data;
using DoFramework.Domain;
using DoFramework.Environment;
using DoFramework.FileSystem;
using DoFramework.Logging;
using DoFramework.Processing;
using DoFramework.Services;
using FluentAssertions;
using Moq;

namespace DoFrameworkTests.Processing;

public class ComposerOrchestratorTests
{
    [Theory]
    [InlineAutoMoqData]
    public void ComposerOrchestrator_CouldNotFindComposer(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<ISetProcessLocation> setLocation,
        string composerName,
        string path,
        ComposerDescriptor composerDescriptor)
    {
        // Arrange
        var session = new Mock<ISession>();
        var resolver = new Mock<IResolver<ComposerDescriptor>>();
        var builder = new Mock<IComposerBuilder>();
        var logger = new Mock<ILogger>();

        composerDescriptor.Name = composerName;

        var container = new ServiceContainer();

        resolver.Setup(x => x.Resolve(It.IsAny<string>())).Returns(new ResolutionResult<ComposerDescriptor>
        (
            false,
            path,
            composerDescriptor
        ));

        var sut = new ComposerOrchestrator(
            environment.Object,
            setLocation.Object,
            logger.Object,
            builder.Object,
            resolver.Object,
            session.Object);

        // Act
        var result = sut.Orchestrate(composerName, container);

        // Assert
        result.Should().BeFalse();

        resolver.Verify(x => x.Resolve(composerName), Times.Once());

        logger.Verify(x => x.LogFatal($"Could not find composer {composerName}"), Times.Once());
    }

    [Theory]
    [InlineAutoMoqData]
    public void ComposerOrchestrator_HandlesException(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<ISetProcessLocation> setLocation,
        string composerName,
        string message,
        ComposerDescriptor composerDescriptor)
    {
        // Arrange
        var session = new Mock<ISession>();
        var resolver = new Mock<IResolver<ComposerDescriptor>>();
        var builder = new Mock<IComposerBuilder>();
        var logger = new Mock<ILogger>();

        composerDescriptor.Name = composerName;

        var container = new ServiceContainer();

        resolver.Setup(x => x.Resolve(It.IsAny<string>())).Throws(new Exception(message));

        var sut = new ComposerOrchestrator(
            environment.Object,
            setLocation.Object,
            logger.Object,
            builder.Object,
            resolver.Object,
            session.Object);

        // Act
        var result = sut.Orchestrate(composerName, container);

        // Assert
        result.Should().BeFalse();

        resolver.Verify(x => x.Resolve(composerName), Times.Once());

        logger.Verify(x => x.LogFatal($"Error building composer: {composerName}"), Times.Once());
        logger.Verify(x => x.LogFatal(message), Times.Once());

        setLocation.Verify(x => x.Set(It.IsAny<string>()), Times.Once);
    }
    [Theory]
    [InlineAutoMoqData]
    public void ComposerOrchestrator_ExecsComposer(
        [Frozen] Mock<IEnvironment> environment,
        [Frozen] Mock<ISetProcessLocation> setLocation,
        string composerName,
        string path,
        ComposerDescriptor composerDescriptor)
    {
        // Arrange
        var composer = new Mock<IComposer>();
        var session = new Mock<ISession>();
        var resolver = new Mock<IResolver<ComposerDescriptor>>();
        var builder = new Mock<IComposerBuilder>();
        var logger = new Mock<ILogger>();

        composerDescriptor.Name = composerName;

        var container = new ServiceContainer();

        resolver.Setup(x => x.Resolve(It.IsAny<string>())).Returns(new ResolutionResult<ComposerDescriptor>
        (
            true,
            path,
            composerDescriptor
        ));

        builder.Setup(x => x.Build(composerDescriptor)).Returns(composer.Object);

        var sut = new ComposerOrchestrator(
            environment.Object,
            setLocation.Object,
            logger.Object,
            builder.Object,
            resolver.Object,
            session.Object);

        // Act
        var result = sut.Orchestrate(composerName, container);

        // Assert
        result.Should().BeTrue();

        resolver.Verify(x => x.Resolve(composerName), Times.Once());

        builder.Verify(x => x.Build(composerDescriptor), Times.Once());

        composer.Verify(x => x.Compose(It.IsAny<IComposerWorkBench>()), Times.Once);

        session.VerifySet(x => x.ComposedBy = composerName, Times.Once);

        setLocation.Verify(x => x.Set(It.IsAny<string>()), Times.Once);
    }
}
