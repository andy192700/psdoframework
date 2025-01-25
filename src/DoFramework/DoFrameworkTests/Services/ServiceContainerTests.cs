using DoFramework.Services;
using FluentAssertions;

namespace DoFrameworkTests.Services;

public class ServiceContainerTests
{
    [Fact]
    public void ServiceContainer_CanBeResolved()
    {
        // Arrange
        var sut = new ServiceContainer();

        // Act
        var result = sut.GetService<IServiceContainer>();

        // Assert
        result.Should().NotBeNull();
        result.GetType().Should().BeAssignableTo(typeof(IServiceContainer));

        sut.Services.Should().HaveCount(1);

        sut.Instances.Should().HaveCount(1);
    }

    [Theory]
    [InlineAutoMoqData]
    public void ServiceContainer_ImplandAbstractionTypesMustNotMatch(Type type)
    {
        // Arrange
        var sut = new ServiceContainer();

        // Act
        var func = () => sut.RegisterService(type, type);

        // Assert
        func.Should().Throw<Exception>().WithMessage($"Implementation and abstraction types must not be the same, failed to register {type.FullName}");
    }

    [Fact]
    public void ServiceContainer_ImplandAbstractionTypesMustNotMatchGeneric()
    {
        // Arrange
        var sut = new ServiceContainer();

        // Act
        var func = () => sut.RegisterService<ExampleService, ExampleService>();

        // Assert
        func.Should().Throw<Exception>().WithMessage($"Implementation and abstraction types must not be the same, failed to register {typeof(ExampleService).FullName}");
    }

    [Fact]
    public void ServiceContainer_ImplandAbstractionTypesMustDerive()
    {
        // Arrange
        var sut = new ServiceContainer();

        // Act
        var func = () => sut.RegisterService(typeof(ExampleService), typeof(ExampleService2));

        // Assert
        func.Should().Throw<Exception>().WithMessage($"Service of Type {typeof(ExampleService2).FullName} does derive from {typeof(ExampleService).FullName}");
    }

    [Theory]
    [InlineAutoMoqData]
    public void ServiceContainer_CannotGetServiceNotResolved(Type type)
    {
        // Arrange
        var sut = new ServiceContainer();

        // Act
        var func = () => sut.GetService(type);

        // Assert
        func.Should().Throw<Exception>().WithMessage($"Service of Type '{type}' could not be resolved.");
    }

    [Fact]
    public void ServiceContainer_CannotGetServiceNotResolvedGeneric()
    {
        // Arrange
        var sut = new ServiceContainer();

        // Act
        var func = () => sut.GetService<ExampleService>();

        // Assert
        func.Should().Throw<Exception>().WithMessage($"Service of Type '{typeof(ExampleService)}' could not be resolved.");
    }

    [Theory]
    [InlineAutoMoqData]
    public void ServiceContainer_CannotRegisterServiceTwice(Type type)
    {
        // Arrange
        var sut = new ServiceContainer();

        sut.RegisterService(type);

        // Act
        var func = () => sut.RegisterService(type);

        // Assert
        func.Should().Throw<Exception>().WithMessage($"Service of Type '{type}' already exists in the container.");
    }

    [Fact]
    public void ServiceContainer_CannotRegisterServiceTwiceGeneric()
    {
        // Arrange
        var sut = new ServiceContainer();

        sut.RegisterService<ExampleService>();

        // Act
        var func = () => sut.RegisterService<ExampleService>();

        // Assert
        func.Should().Throw<Exception>().WithMessage($"Service of Type '{typeof(ExampleService)}' already exists in the container.");
    }

    [Fact]
    public void ServiceContainer_CannotResolveNoConstructors()
    {
        // Arrange
        var sut = new ServiceContainer();

        sut.RegisterService<ExampleService3>();

        // Act
        var func = () => sut.GetService<ExampleService3>();

        // Assert
        func.Should().Throw<Exception>().WithMessage($"Service of type {typeof(ExampleService3)} could not be initalised, could not find any constructors.");
    }

    [Fact]
    public void ServiceContainer_CannotResolveMultipleConstructors()
    {
        // Arrange
        var sut = new ServiceContainer();

        sut.RegisterService<ExampleService4>();

        // Act
        var func = () => sut.GetService<ExampleService4>();

        // Assert
        func.Should().Throw<Exception>().WithMessage($"Service of type {typeof(ExampleService4)} could not be initalised, only one constructor is allowed.");
    }

    [Fact]
    public void ServiceContainer_ResolvesServices()
    {
        // Arrange / Act
        var sut = new ServiceContainer();

        sut.RegisterService<ExampleService>();
        sut.RegisterService<ExampleService2>();

        //Assert
        sut.Instances.Should().HaveCount(1);
        sut.Services.Should().HaveCount(3);
        sut.GetService<ExampleService>().Should().NotBeNull();
        sut.GetService<ExampleService2>().Should().NotBeNull();
        sut.Instances.Should().HaveCount(3);
    }

    [Fact]
    public void ServiceContainer_ResolvesMultipleServices()
    {
        // Arrange
        var sut = new ServiceContainer();

        sut.RegisterService<ExampleInterface<int>, ExampleService>();
        sut.RegisterService<ExampleInterface<string>, ExampleService2>();

        //Assert
        sut.Instances.Should().HaveCount(1);
        sut.Services.Should().HaveCount(3);

        var services = sut.GetServicesByType<ExampleInterface>();
        
        services.Should().HaveCount(2);
        sut.Instances.Should().HaveCount(3);

        services.Any(x => x.GetType() == typeof(ExampleService)).Should().BeTrue();
        services.Any(x => x.GetType() == typeof(ExampleService2)).Should().BeTrue();
    }
}

public interface ExampleInterface { }

public interface ExampleInterface<T> : ExampleInterface { }

public class ExampleService : ExampleInterface<int> { }

public class ExampleService2 : ExampleInterface<string> { }

public class ExampleService3 
{ 
    private ExampleService3() { }
}

public class ExampleService4
{
    public ExampleService4() { }
#pragma warning disable
    public ExampleService4(string parameter) { }
#pragma warning enable
}