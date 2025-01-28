using DoFramework.Processing;
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

    [Fact]
    public void ConfiguresObject_AllPropertiesSet()
    {
        // Arrange
        var sut = new ServiceContainer();

        sut.RegisterService<ISession, Session>();
        sut.RegisterService<IContext, Context>();

        var context = sut.GetService<IContext>();
        context.AddOrUpdate("ExampleType.myInt", 3);
        context.AddOrUpdate("ExampleType.myFloat", 2.2f);
        context.AddOrUpdate("ExampleType.myDouble", 3.5);
        context.AddOrUpdate("ExampleType.myBool", true);
        context.AddOrUpdate("ExampleType.myChar", 'a');
        context.AddOrUpdate("ExampleType.myByte", (byte)8);
        context.AddOrUpdate("ExampleType.myShort", (short)9);
        context.AddOrUpdate("ExampleType.myLong", 4444444L);
        context.AddOrUpdate("ExampleType.myDecimal", 1.14m);
        context.AddOrUpdate("ExampleType.myString", "exampleString");

        // Act
        sut.Configure<ExampleType>();

        var result = sut.GetService<ExampleType>();

        // Assert
        result.myInt.Should().Be(3);
        result.myFloat.Should().Be(2.2f);
        result.myDouble.Should().Be(3.5);
        result.myBool.Should().Be(true);
        result.myChar.Should().Be('a');
        result.myByte.Should().Be((byte)8);
        result.myShort.Should().Be((short)9);
        result.myLong.Should().Be(4444444L);
        result.myDecimal.Should().Be(1.14m);
        result.myString.Should().Be("exampleString");
    }

    [Fact]
    public void ConfiguresObject_NoPropertiesSet()
    {
        // Arrange
        var sut = new ServiceContainer();

        sut.RegisterService<ISession, Session>();
        sut.RegisterService<IContext, Context>();

        var context = sut.GetService<IContext>();

        // Act
        sut.Configure<ExampleType>();

        var result = sut.GetService<ExampleType>();

        // Assert
        result.myInt.Should().Be(default);
        result.myFloat.Should().Be(default);
        result.myDouble.Should().Be(default);
        result.myBool.Should().Be(default);
        result.myChar.Should().Be(default);
        result.myByte.Should().Be(default);
        result.myShort.Should().Be(default);
        result.myLong.Should().Be(default);
        result.myDecimal.Should().Be(default);
        result.myString.Should().Be(default);
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

public class ExampleType
{
    public int myInt;
    public float myFloat;
    public double myDouble;
    public bool myBool;
    public char myChar;
    public byte myByte;
    public short myShort;
    public long myLong;
    public decimal myDecimal;
    public string myString;
}