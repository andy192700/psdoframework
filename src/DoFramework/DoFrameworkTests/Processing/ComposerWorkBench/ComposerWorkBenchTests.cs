using DoFramework.Processing;
using DoFramework.Services;
using DoFramework.Validators;
using FluentAssertions;

namespace DoFrameworkTests.Processing;

public class ComposerWorkBenchTests
{
    [Fact]
    public void ComposerWorkBench_RegistersServices()
    {
        // Arrange
        var container = new ServiceContainer();

        var sut = new ComposerWorkBench(container);

        // Act
        sut.RegisterService(typeof(ComposerTypeValidator)).
            And(typeof(ProcessTypeValidator));

        // Assert
        var service = sut.GetService(typeof(ComposerTypeValidator));
        service.Should().NotBeNull();
        service.Should().BeOfType<ComposerTypeValidator>();

        var service2 = sut.GetService(typeof(ProcessTypeValidator));
        service2.Should().NotBeNull();
        service2.Should().BeOfType<ProcessTypeValidator>();
    }

    [Fact]
    public void ComposerWorkBench_RegistersImplServices()
    {
        // Arrange
        var container = new ServiceContainer();

        var sut = new ComposerWorkBench(container);

        // Act
        sut.RegisterService(typeof(ISession), typeof(Session)).
            And(typeof(IContext), typeof(Context));

        // Assert
        var service = sut.GetService(typeof(ISession));
        service.Should().NotBeNull();
        service.Should().BeOfType<Session>();

        var service2 = sut.GetService(typeof(IContext));
        service2.Should().NotBeNull();
        service2.Should().BeOfType<Context>();
    }

    [Theory]
    [InlineAutoMoqData]
    public void ComposerWorkBench_RegistersProcesses(string process1, string process2)
    {
        // Arrange
        var container = new ServiceContainer();

        var sut = new ComposerWorkBench(container);

        sut.RegisterService(typeof(IProcessRegistry), typeof(ProcessRegistry));

        // Act
        sut.RegisterProcess(process1).
            And(process2);

        // Assert
        var registry = sut.GetService(typeof(IProcessRegistry));

        registry.Should().NotBeNull();
        registry.Should().BeOfType(typeof(ProcessRegistry));

        var request = ((IProcessRegistry)registry).ToProcessRequest();

        request.Processes.Should().HaveCount(2);
        request.Processes[0].Should().Be(process1);
        request.Processes[1].Should().Be(process2);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ComposerWorkBench_ConfiguresObject(string string1, int int1)
    {
        // Arrange
        var container = new ServiceContainer();

        var sut = new ComposerWorkBench(container);

        sut.RegisterService(typeof(ISession), typeof(Session)).
            And(typeof(IContext), typeof(Context));

        container.GetService<IContext>().AddOrUpdate("ConfigurableClass.MyString1", string1);
        container.GetService<IContext>().AddOrUpdate("ConfigurableClass.MyInt1", int1);

        // Act
        sut.Configure(typeof(ConfigurableClass));

        // Assert
        var result = container.GetService<ConfigurableClass>();
        result.Should().NotBeNull();
        result.MyString1 = string1;
        result.MyInt1 = int1;

    }
}

public class ConfigurableClass
{
    public string? MyString1 { get; set; }
    public int MyInt1 { get; set; }
}