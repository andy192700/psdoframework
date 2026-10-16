using DoFramework.Processing;
using DoFramework.Services;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace DoFrameworkTests.Services;

public class ServiceContainerTests
{
    [Fact]
    public void ServiceContainer_CanBeResolved()
    {
        // Arrange
        var sut = new ServiceContainer();

        // Act
        var result = sut.GetService<IReadOnlyServiceContainer>();

        // Assert
        result.Should().NotBeNull();
        result.GetType().Should().BeAssignableTo(typeof(IReadOnlyServiceContainer));
        result.GetService<IReadOnlyServiceContainer>().Should().NotBeNull();
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
    public void ServiceContainer_ResolvesServices()
    {
        // Arrange / Act
        var sut = new ServiceContainer();

        sut.RegisterService<ExampleService>();
        sut.RegisterService<ExampleService2>();

        //Assert
        sut.GetService<ExampleService>().Should().NotBeNull();
        sut.GetService<ExampleService2>().Should().NotBeNull();
    }

    [Fact]
    public void ServiceContainer_ResolvesMultipleServices()
    {
        // Arrange
        var sut = new ServiceContainer();

        sut.RegisterService<ExampleInterface<int>, ExampleService>();
        sut.RegisterService<ExampleInterface<string>, ExampleService2>();

        //Assert
        var services = sut.GetServicesByType<ExampleInterface>();
        
        services.Should().HaveCount(2);

        services.Any(x => x.GetType() == typeof(ExampleService)).Should().BeTrue();
        services.Any(x => x.GetType() == typeof(ExampleService2)).Should().BeTrue();
    }

    [Fact]
    public void ServiceContainer_CannotHonourAConstructor()
    {
        // Arrange
        var sut = new ServiceContainer();

        sut.RegisterService<ExampleService4>();

        // Act
        var func = () => sut.GetService<ExampleService4>();

        // Assert
        func.Should().Throw<Exception>().WithMessage($"Service of type {typeof(ExampleService4)} could not be initalised, service container could not honour a constructor.");
    }

    [Fact]
    public void ServiceContainer_InitialisesLargestConstructor()
    {
        // Arrange
        var sut = new ServiceContainer();

        sut.RegisterService<ExampleService5>();
        sut.RegisterService<ExampleService>();
        sut.RegisterService<ExampleService2>();

        // Act
        var result = sut.GetService<ExampleService5>();

        // Assert
        result.Should().NotBeNull();
        result.ExampleService.Should().NotBeNull();
        result.ExampleService2.Should().NotBeNull();
    }

    [Fact]
    public void ServiceContainer_InitialisesSmallestConstructor()
    {
        // Arrange
        var sut = new ServiceContainer();

        sut.RegisterService<ExampleService5>();
        sut.RegisterService<ExampleService>();

        // Act
        var result = sut.GetService<ExampleService5>();

        // Assert
        result.Should().NotBeNull();
        result.ExampleService.Should().NotBeNull();
        result.ExampleService2.Should().BeNull();
    }

    [Fact]
    public void ServiceContainer_CannotHonourChildServiceConstructorOfRequestedService()
    {
        // Arrange
        var sut = new ServiceContainer();

        sut.RegisterService<ExampleService7>();
        sut.RegisterService<ExampleService4>();

        // Act
        var func = () => sut.GetService<ExampleService7>();

        // Assert
        func.Should().Throw<Exception>().WithMessage($"Service of type {typeof(ExampleService4)} could not be initalised, service container could not honour a constructor.");
    }

    [Fact]
    public void ServiceContainer_HonoursExplicitDefaultConstructor()
    {
        // Arrange / Act
        var sut = new ServiceContainer();

        sut.RegisterService<ExampleInterface<bool>, ExampleService8>();

        // Act
        var result = sut.GetService<ExampleInterface<bool>>();

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ExampleService8>();
    }

    [Fact]
    public void ServiceContainer_HasServiceGenericTrue()
    {
        // Arrange
        var sut = new ServiceContainer();

        sut.RegisterService<ExampleInterface<int>, ExampleService>();

        // Act
        var result = sut.HasService<ExampleInterface<int>>();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ServiceContainer_HasServiceGenericFalse()
    {
        // Arrange
        var sut = new ServiceContainer();

        // Act
        var result = sut.HasService<ExampleInterface<int>>();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ServiceContainer_HasServiceTypeTrue()
    {
        // Arrange
        var sut = new ServiceContainer();

        sut.RegisterService<ExampleInterface<int>, ExampleService>();

        // Act
        var result = sut.HasService(typeof(ExampleInterface<int>));

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ServiceContainer_HasServiceTypeFalse()
    {
        // Arrange
        var sut = new ServiceContainer();

        // Act
        var result = sut.HasService(typeof(ExampleInterface<int>));

        // Assert
        result.Should().BeFalse();
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
#pragma warning disable
    public ExampleService4(ExampleService3 exampleService3) { }
#pragma warning enable
}

public class ExampleService5
{
    public ExampleService ExampleService { get; set; }
    public ExampleService2 ExampleService2 { get; set; }

    public ExampleService5(ExampleService exampleService) 
    {
        ExampleService = exampleService;
    }
    public ExampleService5(ExampleService exampleService, ExampleService2 exampleService2) 
    { 
        ExampleService = exampleService;
        ExampleService2 = exampleService2;
    }
}

public class ExampleService7
{
#pragma warning disable
    public ExampleService7(ExampleService4 exampleService4) { }
#pragma warning enable
}

public class ExampleService8 : ExampleInterface<bool>
{
#pragma warning disable
    public ExampleService8() { }
#pragma warning enable
}

public class ExampleType
{
    public int myInt { get; set; }
    public float myFloat { get; set; }
    public double myDouble { get; set; }
    public bool myBool { get; set; }
    public char myChar { get; set; }
    public byte myByte { get; set; }
    public short myShort { get; set; }
    public long myLong { get; set; }
    public decimal myDecimal { get; set; }
    public string myString { get; set; }
}